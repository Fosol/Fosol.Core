using Fosol.Core.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.JsonConverters
{

    public class PagedListConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(PagedList<>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Get the information for the 'objectType' which will be a PagedList<T>.
            var result_item_property = objectType.GetProperty("Items");
            var result_item_type = result_item_property.PropertyType.GetElementType();

            // Create an instance of a PagedList<T>;
            var page = Activator.CreateInstance(objectType);

            // Get the information for the SerializedPagedList<T> which is the 'existingValue'.
            var gspl_type = typeof(SerializedPageList<>);
            var spl_type = gspl_type.MakeGenericType(result_item_type);
            var spl_method = spl_type.GetMethod("ToPagedList");

            var spl = serializer.Deserialize(reader, spl_type);

            return spl_method.Invoke(spl, null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Get the information for the 'value' which will be a PagedList<T>.
            var value_type = value.GetType();
            var value_items_property = value_type.GetProperty("Items");
            var value_item_type = value_items_property.PropertyType.GetElementType();

            // Get the information for the SerializedPagedList<T>.
            var gspl_type = typeof(SerializedPageList<>);
            var spl_type = gspl_type.MakeGenericType(value_item_type);

            // create a SerializedPagedList<T>.
            var spl = Activator.CreateInstance(spl_type, value);

            // Serialize the SerializedPagedList<T>.
            serializer.Serialize(writer, spl);
        }

        /// <summary>
        /// Use this for serializing the <see cref="PagedList{T}"/> object.
        /// </summary>
        /// <typeparam name="ST"></typeparam>
        public sealed class SerializedPageList<ST>
        {
            #region Properties
            public int Total { get; set; }

            public List<ST> Items { get; set; } = new List<ST>();
            #endregion

            #region Constructors
            public SerializedPageList()
            {

            }

            public SerializedPageList(PagedList<ST> page)
            {
                this.Total = page.Total;
                this.Items = page.Items.ToList();
            }
            #endregion

            #region Methods
            public static explicit operator SerializedPageList<ST>(PagedList<ST> page)
            {
                return new SerializedPageList<ST>(page);
            }

            public static explicit operator PagedList<ST>(SerializedPageList<ST> page)
            {
                return new PagedList<ST>(page.Items, page.Total);
            }

            public PagedList<ST> ToPagedList()
            {
                return new PagedList<ST>(this.Items, this.Total);
            }
            #endregion
        }
    }
}


//public class PagedListTests
//{
//    public static IEnumerable<object[]> GetData()
//    {
//        yield return new object[] { new PagedList<int>(new[] { 4, 5, 6 }, 10) };
//        yield return new object[] { new PagedList<string>(new[] { "test", "32", "634" }, 54) };
//        yield return new object[] { new PagedList<Models.Source>(new[] { new Models.Source() { Id = "Test", Name = "test" } }, 54) };
//    }

//    public static IEnumerable<object[]> GetJson()
//    {
//        yield return new object[] { typeof(PagedList<int>), "{\"Total\":10, \"Items\": [1, 3, 5, 6]}", 4, 10 };
//        yield return new object[] { typeof(PagedList<string>), "{\"Total\":5, \"Items\": [\"1\", \"3\", \"5\", \"6\"]}", 4, 5 };
//        yield return new object[] { typeof(PagedList<Models.Source>), "{\"Total\":3, \"Items\": [ { \"Id\": \"Test\", \"Name\": \"test\" } ]}", 1, 3 };
//    }

//    [Theory]
//    [MemberData(nameof(GetData))]
//    public void Serialize(object page)
//    {
//        var json = Newtonsoft.Json.JsonConvert.SerializeObject(page);

//        Assert.True(json != null);
//    }

//    [Theory]
//    [MemberData(nameof(GetJson))]
//    public void Deserialize(Type type, string json, int count, int total)
//    {
//        dynamic page = Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);

//        Assert.True(page != null);
//        Assert.True(page.Items.Count == count);
//        Assert.True(page.Total == total);
//        Assert.IsType(type, page);
//    }
//}