using Fosol.Core.Extensions.EventHandlers;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Fosol.Core.Configuration
{

    /// <summary>
    /// ConfigurationSectionFileWatcher generic class of type T, where T is of type ConfigurationSection.
    /// Provides a way to watch a specific configuration section file for changes.
    /// If a change occurs with the section it will fire the appropriate event.
    /// Remember to call the Start method to begin watching the configuration file.
    /// </summary>
    /// <typeparam name="T">Type of ConfigurationSection object being watched.</typeparam>
    public class ConfigurationSectionFileWatcher<T> 
        : IDisposable
        where T : ConfigurationSection, new()
    {
        #region Variables
        protected readonly object _BigLock = new object();
        private readonly System.Threading.ReaderWriterLockSlim _Lock = new System.Threading.ReaderWriterLockSlim();
        private readonly bool _IsExternalConfig;
        private FileSystemWatcher _Watcher;
        private bool _IsConfigLoaded = false;
        private bool _IsWatching = false;
        private System.Configuration.Configuration _Configuration;
        private T _ConfigurationSection;
        private string _Filename;
        private bool _ThrowOnError = true;

        /// <summary>
        /// Fires when the configuration fails to load.
        /// </summary>
        public event EventHandler<Events.ConfigurationSectionErrorEventArgs> Error;

        /// <summary>
        /// Fires when the configuration file has changed.
        /// </summary>
        public event EventHandler<FileSystemEventArgs> FileChanged;

        /// <summary>
        /// Fires when the configuration file is created.
        /// </summary>
        public event EventHandler<FileSystemEventArgs> FileCreated;

        /// <summary>
        /// Fires when the configuration file is deleted.
        /// </summary>
        public event EventHandler<FileSystemEventArgs> FileDeleted;

        /// <summary>
        /// Fires when the configuration file is renamed.
        /// </summary>
        public event EventHandler<RenamedEventArgs> FileRenamed;
        #endregion

        #region Properties
        /// <summary>
        /// get - The name of the configuration section.
        /// </summary>
        public string SectionName { get; private set; }

        /// <summary>
        /// get - Whether the configuration file is an full external System.Configuration file.
        /// </summary>
        private bool IsExternalConfig
        {
            get { return _IsExternalConfig; }
        }

        /// <summary>
        /// get - Whether the configuration file has been loaded.
        /// </summary>
        protected bool IsConfigLoaded
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _IsConfigLoaded;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _IsConfigLoaded = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get - Application reference to System.Configuration object.
        /// </summary>
        public System.Configuration.Configuration Configuration
        {
            get
            {
                if (_Configuration == null)
                    return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                return _Configuration;
            }
            protected set { _Configuration = value; }
        }

        /// <summary>
        /// get - Access the ConfigurationSection object.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Configuration Section did not exist.</exception>
        public T Section
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    if (_IsConfigLoaded)
                        return _ConfigurationSection;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }

                return null;
            }
            protected set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _ConfigurationSection = value;

                    if (value != null)
                        _IsConfigLoaded = true;
                    else
                        _IsConfigLoaded = false;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// get - Path to configuration file.
        /// </summary>
        public string Filename
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _Filename;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            protected set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _Filename = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Whether exceptions that occur during the LoadConfig or RefreshSection are thrown as exceptions or whether they fire the ConfigurationError event instead.
        /// By default it will throw the exception as per usual coding standards.
        /// </summary>
        public bool ThrowOnError
        {
            get
            {
                _Lock.EnterReadLock();
                try
                {
                    return _ThrowOnError;
                }
                finally
                {
                    _Lock.ExitReadLock();
                }
            }
            set
            {
                _Lock.EnterWriteLock();
                try
                {
                    _ThrowOnError = value;
                }
                finally
                {
                    _Lock.ExitWriteLock();
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ConfigurationSectionFileWatcher object.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "sectionNameOrFilePath" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "sectionNameOrFilePath" cannot be null.</exception>
        /// <param name="sectionNameOrFilename">Full path to the section configuration file, or the section name of the configuration.</param>
        public ConfigurationSectionFileWatcher(string sectionNameOrFilename)
            : base()
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(sectionNameOrFilename, "sectionNameOrFilename");

            var full_path = System.IO.Path.Combine(Environment.CurrentDirectory, sectionNameOrFilename);

            // Check to see if the file exists at the specified path.  If it doesn't assume this is a section name instead.
            if (System.IO.File.Exists(sectionNameOrFilename))
                this.Filename = sectionNameOrFilename;
            else if (System.IO.File.Exists(full_path))
                this.Filename = full_path;
            else
                this.SectionName = sectionNameOrFilename;
        }

        /// <summary>
        /// Creates a new instance of a ConfigurationSectionFileWatcher object.
        /// 
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameters "externalConfigPath" and "sectionName" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameters "externalConfigPath" and "sectionName" cannot be null.</exception>
        /// <param name="externalConfigFilename">Path to the external System.Configuration file.</param>
        /// <param name="sectionName">Name of the custom section within the configuraiton file.</param>
        public ConfigurationSectionFileWatcher(string externalConfigFilename, string sectionName)
        {
            Validation.Argument.Assert.IsNotNullOrEmpty(externalConfigFilename, "externalConfigFilename");
            Validation.Argument.Assert.IsNotNullOrEmpty(sectionName, "sectionName");

            var full_path = System.IO.Path.Combine(Environment.CurrentDirectory, externalConfigFilename);
            if (System.IO.File.Exists(externalConfigFilename))
                this.Filename = externalConfigFilename;
            else
                this.Filename = full_path;

            this.SectionName = sectionName;
            _IsExternalConfig = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Raises ConfigurationError event.
        /// Provides a way for inherited classes to receive the ConfigurationError event.
        /// </summary>
        /// <param name="ex">Exception that was thrown.</param>
        protected virtual void OnError(Exception ex)
        {
            if (this.ThrowOnError)
                throw ex;
            else
                this.Error.Raise(this, new Events.ConfigurationSectionErrorEventArgs(ex));
        }

        /// <summary>
        /// Deserialize the section configuration into the ConfigurationSection of object of type T.
        /// </summary>
        /// <exception cref="System.ArgumentException">Parameter "path" cannot be empty.</exception>
        /// <exception cref="System.ArgumentNullException">Parameter "path" cannot be null.</exception>
        /// <exception cref="System.IO.FileNotFoundException">Section configuration file must exist.</exception>
        /// <param name="filename">Path to section configuration file.</param>
        /// <returns>ConfigurationSection object of type T.</returns>
        protected static T DeserializeSection(string filename)
        {
            Validation.Argument.Assert.IsNotNull(filename, nameof(filename));

            if (!File.Exists(filename))
                throw new System.IO.FileNotFoundException(String.Format(Resources.Multilingual.ConfigurationSectionFileWatcher_DeserializeSection_FileNotFound, nameof(filename), Path.GetFileName(filename)), filename);

            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    var section = new T();
                    section.GetType().GetMethod(nameof(DeserializeSection), BindingFlags.NonPublic | BindingFlags.Instance).Invoke(section, new object[] { reader });
                    return section;
                }
            }
        }

        /// <summary>
        /// If the configuration file hasn't been loaded it will attempt to load it before starting the FileSystemWatcher.
        /// Start the FileSystemWatcher object.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Configuration Section did not exist.</exception>
        public void Start()
        {
            if (!this.IsConfigLoaded)
                LoadConfig();

            lock (_BigLock)
            {
                // Only start the watcher if it hasn't already started.
                if (!_IsWatching && _IsConfigLoaded)
                {
                    if (_Watcher == null)
                    {
                        var path = Path.GetDirectoryName(this.Filename);

                        if (!string.IsNullOrEmpty(path))
                            _Watcher = new FileSystemWatcher(path, Path.GetFileName(this.Filename));
                        else
                            _Watcher = new FileSystemWatcher(Environment.CurrentDirectory, Path.GetFileName(this.Filename));
                    }
                    _Watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
                    _Watcher.Created += OnFileCreated;
                    _Watcher.Changed += OnFileChanged;
                    _Watcher.Deleted += OnFileDeleted;
                    _Watcher.Renamed += OnFileRenamed;
                    _Watcher.EnableRaisingEvents = true;

                    _IsWatching = true;
                }
            }
        }

        /// <summary>
        /// Refresh and load the configuration section.
        /// Raise FileChanged event.
        /// </summary>
        /// <param name="sender">Object sending event.</param>
        /// <param name="e">Event argument to include with event.</param>
        protected void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                this._Watcher.EnableRaisingEvents = false;
                RefreshSection();
            }
            catch (Exception ex)
            {
                this.Error.Raise(sender, new Events.ConfigurationSectionErrorEventArgs(ex, e));
            }
            finally
            {
                this.FileChanged.Raise(sender, e);
                this._Watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Refresh and load the configuration section.
        /// Raise FileCreated event.
        /// </summary>
        /// <param name="sender">Object sending event.</param>
        /// <param name="e">Event argument to include with event.</param>
        protected void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                this._Watcher.EnableRaisingEvents = false;
                RefreshSection();
            }
            catch (Exception ex)
            {
                this.Error.Raise(sender, new Events.ConfigurationSectionErrorEventArgs(ex, e));
            }
            finally
            {
                this.FileCreated.Raise(sender, e);
                this._Watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Refresh and load the configuration section.
        /// Raise FileDeleted event.
        /// </summary>
        /// <param name="sender">Object sending event.</param>
        /// <param name="e">Event argument to include with event.</param>
        protected void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                this._Watcher.EnableRaisingEvents = false;
                RefreshSection();
            }
            catch (Exception ex)
            {
                this.Error.Raise(sender, new Events.ConfigurationSectionErrorEventArgs(ex, e));
            }
            finally
            {
                this.FileDeleted.Raise(sender, e);
                this._Watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Refresh and load the configuration section.
        /// Raise FileRenamed event.
        /// </summary>
        /// <param name="sender">Object sending event.</param>
        /// <param name="e">Event argument to include with event.</param>
        protected void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            try
            {
                this._Watcher.EnableRaisingEvents = false;
                RefreshSection();
            }
            catch (Exception ex)
            {
                this.Error.Raise(sender, new Events.ConfigurationSectionErrorEventArgs(ex, e));
            }
            finally
            {
                this.FileRenamed.Raise(sender, e);
                this._Watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Refresh and load the configuration section.
        /// </summary>
        protected void RefreshSection()
        {
            lock (_BigLock)
            {
                if (this.IsConfigLoaded)
                {
                    this.IsConfigLoaded = false;
                    ConfigurationManager.RefreshSection(this.SectionName);
                    this.Section = null;
                    LoadConfigWithoutLock();
                }
            }
        }

        /// <summary>
        /// Load the configuration section.
        /// Lock object to ensure thread safety.
        /// If the SectionName is specified it will attempt to load the section the standard way.
        /// If the SectionName is not specified it will attempt to deserialize the file at the FilePath.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Configuration Section did not exist.</exception>
        protected void LoadConfig()
        {
            lock (_BigLock)
            {
                LoadConfigWithoutLock();
            }
        }

        /// <summary>
        /// Load the configuration section.
        /// If the SectionName is specified it will attempt to load the section the standard way.
        /// If the SectionName is not specified it will attempt to deserialize the file at the FilePath.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Configuration Section did not exist.</exception>
        private void LoadConfigWithoutLock()
        {
            try
            {
                if (this.IsExternalConfig)
                {
                    // This is pointing to an external System.Configuration file which will contain a section within it.
                    if (!System.IO.File.Exists(this.Filename)
                        || !System.IO.File.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, this.Filename)))
                        throw new ConfigurationErrorsException(String.Format(Resources.Multilingual.ConfigurationSectionFileWatcher_LoadConfigWithoutLock_FileNotFound, this.Filename));

                    var file_map = new ConfigurationFileMap(this.Filename);
                    this.Configuration = ConfigurationManager.OpenMappedMachineConfiguration(file_map);
                    this.Section = (T)this.Configuration.GetSection(this.SectionName);

                    if (this.Section == null)
                        throw new ConfigurationErrorsException(String.Format(Resources.Multilingual.ConfigurationSectionFileWatcher_LoadConfigWithoutLock_SectionNotFound, this.SectionName));
                }
                else if (!string.IsNullOrEmpty(this.SectionName))
                {
                    this.Section = (T)Configuration.GetSection(this.SectionName);

                    if (this.Section == null)
                        throw new ConfigurationErrorsException(String.Format(Resources.Multilingual.ConfigurationSectionFileWatcher_LoadConfigWithoutLock_SectionNotFound, this.SectionName));

                    // Set the FilePath if it hasn't already been set.
                    if (string.IsNullOrEmpty(this.Filename))
                    {
                        if (!string.IsNullOrEmpty(this.Section.SectionInformation.ConfigSource))
                            this.Filename = this.Section.SectionInformation.ConfigSource;
                        else
                            this.Filename = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                    }
                }
                else
                {
                    // The external independant section configuration file.
                    this.Section = ConfigurationSectionFileWatcher<T>.DeserializeSection(this.Filename);

                    if (this.Section == null)
                        throw new ConfigurationErrorsException(string.Format(Resources.Multilingual.ConfigurationSectionFileWatcher_LoadConfigWithoutLock_FileNotFound, this.Filename));

                    if (string.IsNullOrEmpty(this.SectionName))
                        this.SectionName = Section.SectionInformation.Name;
                }

                this.IsConfigLoaded = true;
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }
        }

        /// <summary>
        /// Unhook listeners from the FileSystemWatcher.
        /// </summary>
        public void Stop()
        {
            lock (_BigLock)
            {
                if (_IsWatching && _Watcher != null)
                {
                    _Watcher.Created -= OnFileCreated;
                    _Watcher.Changed -= OnFileChanged;
                    _Watcher.Deleted -= OnFileDeleted;
                    _Watcher.Renamed -= OnFileRenamed;
                    _Watcher.EnableRaisingEvents = false;
                    _IsWatching = false;
                }
            }
        }

        /// <summary>
        /// Dispose of the FileSystemWatcher listener events.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
