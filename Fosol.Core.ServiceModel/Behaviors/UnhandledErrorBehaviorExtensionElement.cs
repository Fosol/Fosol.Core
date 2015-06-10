using Fosol.Core.Extensions.Types;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Configuration;

namespace Fosol.Core.ServiceModel.Behaviors
{
    /// <summary>
    /// UnhandledErrorBehaviorExtensionElement provides a configurable option for whole services or endpoints to capture unhandled errors with the 
    /// UnhandledErrorBehavior class.
    /// </summary>
    public sealed class UnhandledErrorBehaviorExtensionElement
        : BehaviorExtensionElement
    {
        #region Variables
        const string _ConfigurationProperty_ErrorHandlerTypeName = "type";
        const string _ConfigurationProperty_ErrorHandlerMethodName = "method";
        Type _ErrorHandlerType;
        #endregion

        #region Properties
        /// <summary>
        /// get - The UnhandledErrorBehavior type.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(UnhandledErrorBehavior); }
        }
        
        /// <summary>
        /// get/set - The static object Type that the unhandled exception will be sent to.
        /// </summary>
        /// <exception cref="System.ArgumentException">ErrorHandlerType must be a static class.</exception>
        [ConfigurationProperty(_ConfigurationProperty_ErrorHandlerTypeName)]
        [TypeConverter(typeof(Fosol.Core.Converters.AssemblyTypeConverter))]
        public Type ErrorHandlerType 
        {
            get { return _ErrorHandlerType; }
            set
            {
                if (value != null)
                {
                    Fosol.Core.Validation.Property.Assert.IsTrue(value.IsStatic(), "ErrorHandlerType", "Type must be static.");
                }

                _ErrorHandlerType = value;
            }
        }

        /// <summary>
        /// get/set - The name of the method (Action) that the exception will be sent to.
        /// </summary>
        [ConfigurationProperty(_ConfigurationProperty_ErrorHandlerMethodName)]
        public string ErrorHandlerMethodName { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        /// <summary>
        /// Creates an UnhandledErrorBehavior, intializes it and returns it.
        /// </summary>
        /// <returns>A new instance of an UnhandledErrorBehavior object.</returns>
        protected override object CreateBehavior()
        {
            this.ErrorHandlerType = (Type)this.ElementInformation.Properties[_ConfigurationProperty_ErrorHandlerTypeName].Value;
            this.ErrorHandlerMethodName = (string)this.ElementInformation.Properties[_ConfigurationProperty_ErrorHandlerMethodName].Value;

            if (this.ErrorHandlerType != null || !string.IsNullOrEmpty(this.ErrorHandlerMethodName))
            {
                // The ErrorHandlerMethod must be a valid method that accepts an exception.
                var method_info = (
                    from m in ErrorHandlerType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                    where m.Name.Equals(ErrorHandlerMethodName, StringComparison.OrdinalIgnoreCase)
                        && m.GetParameters().Count() == 1
                        && m.GetParameters().FirstOrDefault().ParameterType == typeof(Exception)
                        && m.ReturnType == typeof(bool)
                    select m).FirstOrDefault();

                if (method_info == null)
                    throw new NotImplementedException();

                var method = (Func<Exception, bool>)method_info.CreateDelegate(typeof(Func<Exception, bool>));

                return new UnhandledErrorBehavior(method);
            }
            else
                return new UnhandledErrorBehavior();
        }
        #endregion

        #region Operators
        #endregion

        #region Events
        #endregion
    }
}
