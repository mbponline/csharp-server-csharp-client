using Client.Modules.Utils.DAL.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientTests
{
    [TestClass]
    public class DataAdapterTests
    {
        private DataAdapter dataAdapter;

        [TestInitialize()]
        public async Task Startup()
        {
            var baseUrl = "http://localhost:50069";
            var apiUrl = "/api/datasource";
            var metadataCli = await MetadataUtils.GetMetadataAsync(baseUrl, "/api/datasource/metadata");

            this.dataAdapter = new DataAdapter(baseUrl, apiUrl, metadataCli);
        }

        // api/crud/Actors?$count=true
        [TestMethod]
        public async Task TestGetActors()
        {
            var queryObject = new QueryObject()
            {
                Count = true
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Actor", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"], "api/datasource/crud/Actors?count=true&skip=40&top=200");

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 40"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 40);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Actor");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Null);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            //var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["ActorId"], 200);
            Assert.AreEqual(firstItem["FirstName"], "THORA");
            Assert.AreEqual(firstItem["LastName"], "TEMPLE");
            Assert.AreEqual(((DateTime)firstItem["LastUpdate"]).ToString("yyyy-MM-dd"), "2006-02-15");
        }

        // api/crud/Customers?$count=true
        [TestMethod]
        public async Task TestGetCustomers()
        {
            var queryObject = new QueryObject()
            {
                Count = true
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Customer", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"], "api/datasource/crud/Customers?count=true&skip=40&top=599");

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 40"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 40);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Customer");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Null);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            //var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["CustomerId"], 599);
            Assert.AreEqual(firstItem["StoreId"], 2);
            Assert.AreEqual(firstItem["FirstName"], "AUSTIN");
            Assert.AreEqual(firstItem["LastName"], "CINTRON");
        }

        // api/crud/Customers?$select=FirstName,LastName
        [TestMethod]
        public async Task TestGetCustomersSelect()
        {
            var queryObject = new QueryObject()
            {
                Select = new string[] { "FirstName", "LastName" }
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Customer", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"], "api/datasource/crud/Customers?select=FirstName,LastName&skip=40&top=599");

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 40"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 40);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Customer");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Null);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            //var relatedItems = (JObject)resultSerialDataObject["relatedItems"];


            // "should have property 'CustomerId' on 'items[0]'"
            Assert.IsTrue(firstItem.ContainsKey("CustomerId"));

            // "should have property 'FirstName' on 'items[0]'"
            Assert.IsTrue(firstItem.ContainsKey("FirstName"));

            // "should have property 'LastName' on 'items[0]'"
            Assert.IsTrue(firstItem.ContainsKey("LastName"));

            // "should NOT have property 'StoreId' on 'items[0]'"
            Assert.IsFalse(firstItem.ContainsKey("StoreId"));

            // "should NOT have property 'Email' on 'items[0]'"
            Assert.IsFalse(firstItem.ContainsKey("Email"));

            // "should NOT have property 'AddressId' on 'items[0]'"
            Assert.IsFalse(firstItem.ContainsKey("AddressId"));

            // "should NOT have property 'Active' on 'items[0]'"
            Assert.IsFalse(firstItem.ContainsKey("Active"));

            // "should NOT have property 'CreateDate' on 'items[0]'"
            Assert.IsFalse(firstItem.ContainsKey("CreateDate"));

            // "should NOT have property 'LastUpdate' on 'items[0]'"
            Assert.IsFalse(firstItem.ContainsKey("LastUpdate"));


            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["CustomerId"], 599);
            Assert.AreEqual(firstItem["FirstName"], "AUSTIN");
            Assert.AreEqual(firstItem["LastName"], "CINTRON");
        }

        // api/crud/Countries?$expand=Cities
        [TestMethod]
        public async Task TestGetCountriesExpand()
        {
            var queryObject = new QueryObject()
            {
                Expand = new string[] { "Cities" }
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Country", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"], "api/datasource/crud/Countries?expand=Cities&skip=40&top=109");

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 40"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 40);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Country");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should not be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Object);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["CountryId"], 109);
            Assert.AreEqual(firstItem["Name"], "Zambia");
            Assert.AreEqual(((DateTime)firstItem["LastUpdate"]).ToString("yyyy-MM-dd"), "2006-02-15");

            // "the field 'relatedItems' should have property 'City'"
            Assert.IsTrue(relatedItems.ContainsKey("City"));

            // "City' should be an array having length of 1"
            Assert.IsTrue(relatedItems["City"] is JArray);
            Assert.AreEqual(((JArray)relatedItems["City"]).Count, 220);

            var itemsCity = (JArray)relatedItems["City"];
            var firstCity = (JObject)itemsCity[0];

            // "should have a specific value for the first item of 'City' on 'relatedItems'"
            Assert.AreEqual(firstCity["CityId"], 272);
            Assert.AreEqual(firstCity["Name"], "Kitwe");
            Assert.AreEqual(firstCity["CountryId"], 109);
            Assert.AreEqual(((DateTime)firstCity["LastUpdate"]).ToString("yyyy-MM-dd"), "2006-02-15");
        }

        // api/crud/Films?$orderby=Title asc&$expand=FilmActors($expand=Actor),FilmCategories($expand=Category)&$skip=20&$top=10
        [TestMethod]
        public async Task TestGetFilmsOrderExpandSkipTop()
        {
            var queryObject = new QueryObject()
            {
                OrderBy = new string[] { "Title" },
                Expand = new string[] { "FilmActors.Actor", "FilmCategories.Category" },
                Skip = 20,
                Top = 10
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Film", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"].Type, JTokenType.Null);

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 10"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 10);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Film");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should not be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Object);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["FilmId"], 21);
            Assert.AreEqual(firstItem["Title"], "AMERICAN CIRCUS");

            // "the field 'relatedItems' should have property 'FilmActor'"
            Assert.IsTrue(relatedItems.ContainsKey("FilmActor"));

            // "FilmActor' should be an array having length of 52"
            Assert.IsTrue(relatedItems["FilmActor"] is JArray);
            Assert.AreEqual(((JArray)relatedItems["FilmActor"]).Count, 52);

            var itemsFilmActor = (JArray)relatedItems["FilmActor"];
            var firstFilmActor = (JObject)itemsFilmActor[0];

            // "should have a specific value for the first item of 'FilmActor' on 'relatedItems'"
            Assert.AreEqual(firstFilmActor["ActorId"], 25);
            Assert.AreEqual(firstFilmActor["FilmId"], 21);
            Assert.AreEqual(((DateTime)firstFilmActor["LastUpdate"]).ToString("yyyy-MM-dd"), "2006-02-15");

            // "the field 'relatedItems' should have property 'FilmCategory'"
            Assert.IsTrue(relatedItems.ContainsKey("FilmCategory"));

            // "FilmCategory' should be an array having length of 10"
            Assert.IsTrue(relatedItems["FilmCategory"] is JArray);
            Assert.AreEqual(((JArray)relatedItems["FilmCategory"]).Count, 10);

            var itemsFilmCategory = (JArray)relatedItems["FilmCategory"];
            var firstFilmCategory = (JObject)itemsFilmCategory[0];

            // "should have a specific value for the first item of 'FilmCategory' on 'relatedItems'"
            Assert.AreEqual(firstFilmCategory["FilmId"], 21);
            Assert.AreEqual(firstFilmCategory["CategoryId"], 1);
            Assert.AreEqual(((DateTime)firstFilmCategory["LastUpdate"]).ToString("yyyy-MM-dd"), "2006-02-15");

            // "the field 'relatedItems' should have property 'Actor'"
            Assert.IsTrue(relatedItems.ContainsKey("Actor"));

            // "Actor' should be an array having length of 45"
            Assert.IsTrue(relatedItems["Actor"] is JArray);
            Assert.AreEqual(((JArray)relatedItems["Actor"]).Count, 45);

            var itemsActor = (JArray)relatedItems["Actor"];
            var firstActor = (JObject)itemsActor[0];

            // "should have a specific value for the first item of 'Actor' on 'relatedItems'"
            Assert.AreEqual(firstActor["ActorId"], 25);
            Assert.AreEqual(firstActor["FirstName"], "KEVIN");
            Assert.AreEqual(firstActor["LastName"], "BLOOM");
            Assert.AreEqual(((DateTime)firstActor["LastUpdate"]).ToString("yyyy-MM-dd"), "2006-02-15");

            // "the field 'relatedItems' should have property 'Category'"
            Assert.IsTrue(relatedItems.ContainsKey("Category"));

            // "Category' should be an array having length of 7"
            Assert.IsTrue(relatedItems["Category"] is JArray);
            Assert.AreEqual(((JArray)relatedItems["Category"]).Count, 7);

            var itemsCategory = (JArray)relatedItems["Category"];
            var firstCategory = (JObject)itemsCategory[0];

            // "should have a specific value for the first item of 'Category' on 'relatedItems'"
            Assert.AreEqual(firstCategory["CategoryId"], 1);
            Assert.AreEqual(firstCategory["Name"], "Action");
            Assert.AreEqual(((DateTime)firstCategory["LastUpdate"]).ToString("yyyy-MM-dd"), "2006-02-15");
        }

        // api/crud/Films?$orderby=Title asc&$expand=FilmActors($expand=Actor),FilmCategories($expand=Category)&$skip=20&$top=10&$filter=Length ge 80
        [TestMethod]
        public async Task TestGetFilmsOrderExpandSkipTopFilter()
        {
            var queryObject = new QueryObject()
            {
                Filter = "Length >= 80",
                OrderBy = new string[] { "Title ASC" },
                Expand = new string[] { "FilmActors.Actor", "FilmCategories.Category" },
                Skip = 20,
                Top = 10
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Film", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"].Type, JTokenType.Null);

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 10"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 10);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Film");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should not be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Object);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            //var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["FilmId"], 30);
            Assert.AreEqual(firstItem["Title"], "ANYTHING SAVANNAH");

            // ""all objects on 'items'should have the value of property 'Length' >= 80"
            var result = true;
            foreach (var item in items)
            {
                if (!((int)item["Length"] >= 80))
                {
                    result = false;
                    break;
                }
            }
            Assert.IsTrue(result);
        }

        // api/crud/Films?$orderby=Title asc&$expand=FilmActors($expand=Actor),FilmCategories($expand=Category)&$skip=20&$top=10&$filter=FilmActors/any(x: x/Actor ne null)
        [TestMethod]
        public async Task TestGetFilmsOrderExpandSkipTopFilterExpand1()
        {
            var queryObject = new QueryObject()
            {
                FilterExpand = new FilterExpand[] { new FilterExpand { Expand = "FilmActors.Actor", Filter = "1" } },
                OrderBy = new string[] { "Title ASC" },
                Expand = new string[] { "FilmActors.Actor", "FilmCategories.Category" },
                Skip = 20,
                Top = 10
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Film", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"].Type, JTokenType.Null);

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 10"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 10);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Film");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should not be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Object);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            //var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["FilmId"], 4);
            Assert.AreEqual(firstItem["Title"], "AFFAIR PREJUDICE");
        }


        // api/crud/Films?$orderby=Title asc&$expand=FilmActors($expand=Actor),FilmCategories($expand=Category)&$skip=20&$top=10&$filter=FilmActors/any(x: x/Actor/LastName eq 'DAVIS')
        [TestMethod]
        public async Task TestGetFilmsOrderExpandSkipTopFilterExpand2()
        {
            var queryObject = new QueryObject()
            {
                FilterExpand = new FilterExpand[] { new FilterExpand { Expand = "FilmActors.Actor", Filter = "LastName = 'DAVIS'" } },
                OrderBy = new string[] { "Title ASC" },
                Expand = new string[] { "FilmActors.Actor", "FilmCategories.Category" },
                Skip = 20,
                Top = 10
            };

            var resultSerialResponseToken = await this.dataAdapter.QueryAllAsync("Film", queryObject);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"].Type, JTokenType.Null);

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 10"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 10);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Film");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should not be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Object);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            //var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["FilmId"], 275);
            Assert.AreEqual(firstItem["Title"], "EGYPT TENENBAUMS");
        }


        // ===============================================================================================================
        //
        // Test Functions
        //
        // ===============================================================================================================

        // api/operations/GetFilmsWithActors?$expand=FilmActors($expand=Actor),FilmCategories&releaseYear=2006
        [TestMethod]
        public async Task TestGetFilmsWithActorsOperation()
        {
            var queryObject = new QueryObject()
            {
                Expand = new string[] { "FilmActors.Actor", "FilmCategories.Category" }
            };

            var resultSerialResponseToken = await this.dataAdapter.CallEntityFunctionAsync("GetFilmsWithActors", new Dictionary<string, object> { { "releaseYear", 2006 } }, queryObject, "Film", true);
            var resultSerialResponseObject = (JObject)resultSerialResponseToken;

            // "should have property 'nextLink'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("nextLink"));

            // "should have a specific value for 'nextLink'"
            Assert.AreEqual(resultSerialResponseObject["nextLink"], "api/datasource/crud/Films?filter=ReleaseYear='2006'&expand=FilmActors.Actor,FilmCategories.Category&count=true&skip=40&top=1000");

            // "should have property 'data'"
            Assert.IsTrue(resultSerialResponseObject.ContainsKey("data"));

            var resultSerialDataObject = (JObject)resultSerialResponseObject["data"];

            // "should have property 'items' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("items"));

            // "'items' should be an array having length of 40"
            Assert.IsTrue(resultSerialDataObject["items"] is JArray);
            Assert.AreEqual(((JArray)resultSerialDataObject["items"]).Count, 40);

            // "should have property 'entityTypeName' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("entityTypeName"));

            // "the value for 'entityTypeName' field should be 'Loturi'"
            Assert.AreEqual(resultSerialDataObject["entityTypeName"], "Film");

            // "should have property 'relatedItems' on 'data'"
            Assert.IsTrue(resultSerialDataObject.ContainsKey("relatedItems"));

            // "the value for 'relatedItems' should not be null"
            Assert.AreEqual(resultSerialDataObject["relatedItems"].Type, JTokenType.Object);

            var items = (JArray)resultSerialDataObject["items"];
            var firstItem = (JObject)items[0];
            //var relatedItems = (JObject)resultSerialDataObject["relatedItems"];

            // "should have a specific value for 'items[0]'"
            Assert.AreEqual(firstItem["FilmId"], 1000);
            Assert.AreEqual(firstItem["Title"], "ZORRO ARK");
        }


        // ===============================================================================================================
        //
        // Test Actions
        //
        // ===============================================================================================================

        /*
        // api/datasource/operations/TestAction
        [TestMethod]
        public async Task TestTestAction()
        {
            var resultToken = await this.dataAdapter.CallValueActionAsync("TestAction", new Dictionary<string, object> { { "param1", 1 } });
            var resultValue = (JValue)resultToken;

            // TODO: check the result
        }
        */

    }

}
