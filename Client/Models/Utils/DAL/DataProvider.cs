#pragma warning disable SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300

//------------------------------------------------------------------------------
//    This code was auto-generated.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using Client.Models.Utils.DAL.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Models.Utils.DAL
{
    public class DataService : DataServiceBase<LocalViews, RemoteViews, ServiceFunctions, ServiceActions>
    {
        public DataService(Metadata metadata, string baseUrl, string serviceUrl) : base(metadata, baseUrl, serviceUrl)
        {
            this.From = new ServiceLocation<LocalViews, RemoteViews>() { Local = new LocalViews(this.DataContext), Remote = new RemoteViews(this.DataAdapter, this.DataContext, metadata) };
            this.Operation = new ServiceOperation<ServiceFunctions, ServiceActions>() { Function = new ServiceFunctions(this.DataAdapter, this.DataContext, metadata), Action = new ServiceActions(this.DataAdapter, this.DataContext, metadata) };
        }

        public static async Task<DataService> CreateInstanceAsync(string baseUrl, string serviceUrl)
        {
            var metadata = await DataService.GetMetadataAsync(baseUrl, serviceUrl);
            metadata.Namespace = "Client.Models.Utils.DAL";
            return new DataService(metadata, baseUrl, serviceUrl);
        }

    }

    public class ServiceFunctions : OperationsProvider
    {
        public ServiceFunctions(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata) : base(dataAdapter, dataContext, metadata) { }
        
        public async Task<QueryResult<Film>> GetFilmsWithActors(int releaseYear, QueryObject queryObject = null) { return await this.GetEntitiesAsync<Film>("GetFilmsWithActors", new Dictionary<string, object>() { { "int", releaseYear } }, queryObject); }
    }

    public class ServiceActions : OperationsProvider
    {
        public ServiceActions(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata) : base(dataAdapter, dataContext, metadata) { }
        
        public async Task TestAction(int param1) { await this.PostOperationAsync<object>("TestAction", new Dictionary<string, object>() { { "int", param1 } }); }
    }

    public class LocalViews : PropertyList
    {
        public LocalViews(DataContext dataContext) : base(dataContext) { }
        
        public DataViewLocal<Actor> Actors { get { return this.GetPropertyValue<DataViewLocal<Actor>>(); } }
        public DataViewLocal<Address> Addresses { get { return this.GetPropertyValue<DataViewLocal<Address>>(); } }
        public DataViewLocal<Category> Categories { get { return this.GetPropertyValue<DataViewLocal<Category>>(); } }
        public DataViewLocal<City> Cities { get { return this.GetPropertyValue<DataViewLocal<City>>(); } }
        public DataViewLocal<Country> Countries { get { return this.GetPropertyValue<DataViewLocal<Country>>(); } }
        public DataViewLocal<Customer> Customers { get { return this.GetPropertyValue<DataViewLocal<Customer>>(); } }
        public DataViewLocal<Film> Films { get { return this.GetPropertyValue<DataViewLocal<Film>>(); } }
        public DataViewLocal<FilmActor> FilmActors { get { return this.GetPropertyValue<DataViewLocal<FilmActor>>(); } }
        public DataViewLocal<FilmCategory> FilmCategories { get { return this.GetPropertyValue<DataViewLocal<FilmCategory>>(); } }
        public DataViewLocal<FilmText> FilmTexts { get { return this.GetPropertyValue<DataViewLocal<FilmText>>(); } }
        public DataViewLocal<Inventory> Inventories { get { return this.GetPropertyValue<DataViewLocal<Inventory>>(); } }
        public DataViewLocal<Language> Languages { get { return this.GetPropertyValue<DataViewLocal<Language>>(); } }
        public DataViewLocal<Payment> Payments { get { return this.GetPropertyValue<DataViewLocal<Payment>>(); } }
        public DataViewLocal<Rental> Rentals { get { return this.GetPropertyValue<DataViewLocal<Rental>>(); } }
        public DataViewLocal<Staff> Staffs { get { return this.GetPropertyValue<DataViewLocal<Staff>>(); } }
        public DataViewLocal<Store> Stores { get { return this.GetPropertyValue<DataViewLocal<Store>>(); } }
    }

    public class RemoteViews : PropertyList
    {
        public RemoteViews(DataAdapter dataAdapter, DataContext dataContext, Metadata metadata) : base(dataAdapter, dataContext, metadata) { }
        
        public DataViewRemote<Actor> Actors { get { return this.GetPropertyValue<DataViewRemote<Actor>>(); } }
        public DataViewRemote<Address> Addresses { get { return this.GetPropertyValue<DataViewRemote<Address>>(); } }
        public DataViewRemote<Category> Categories { get { return this.GetPropertyValue<DataViewRemote<Category>>(); } }
        public DataViewRemote<City> Cities { get { return this.GetPropertyValue<DataViewRemote<City>>(); } }
        public DataViewRemote<Country> Countries { get { return this.GetPropertyValue<DataViewRemote<Country>>(); } }
        public DataViewRemote<Customer> Customers { get { return this.GetPropertyValue<DataViewRemote<Customer>>(); } }
        public DataViewRemote<Film> Films { get { return this.GetPropertyValue<DataViewRemote<Film>>(); } }
        public DataViewRemote<FilmActor> FilmActors { get { return this.GetPropertyValue<DataViewRemote<FilmActor>>(); } }
        public DataViewRemote<FilmCategory> FilmCategories { get { return this.GetPropertyValue<DataViewRemote<FilmCategory>>(); } }
        public DataViewRemote<FilmText> FilmTexts { get { return this.GetPropertyValue<DataViewRemote<FilmText>>(); } }
        public DataViewRemote<Inventory> Inventories { get { return this.GetPropertyValue<DataViewRemote<Inventory>>(); } }
        public DataViewRemote<Language> Languages { get { return this.GetPropertyValue<DataViewRemote<Language>>(); } }
        public DataViewRemote<Payment> Payments { get { return this.GetPropertyValue<DataViewRemote<Payment>>(); } }
        public DataViewRemote<Rental> Rentals { get { return this.GetPropertyValue<DataViewRemote<Rental>>(); } }
        public DataViewRemote<Staff> Staffs { get { return this.GetPropertyValue<DataViewRemote<Staff>>(); } }
        public DataViewRemote<Store> Stores { get { return this.GetPropertyValue<DataViewRemote<Store>>(); } }
    }

    public sealed class Actor : IDerivedEntity
    {
        public Actor(Entity entity)
        {
            if (entity.entityTypeName != "Actor") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short ActorId { get { return (short)(long)this.entity.dto["ActorId"]; } set { this.entity.dto["ActorId"] = value; } }
        public string FirstName { get { return (string)this.entity.dto["FirstName"]; } set { this.entity.dto["FirstName"] = value; } }
        public string LastName { get { return (string)this.entity.dto["LastName"]; } set { this.entity.dto["LastName"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<FilmActor> FilmActors { get { return this.entity.NavigateMulti<FilmActor>("Actor", "FilmActors"); } }
        
    }

    public sealed class Address : IDerivedEntity
    {
        public Address(Entity entity)
        {
            if (entity.entityTypeName != "Address") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short AddressId { get { return (short)(long)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = value; } }
        public string Address1 { get { return (string)this.entity.dto["Address1"]; } set { this.entity.dto["Address1"] = value; } }
        public string Address2 { get { return (string)this.entity.dto["Address2"]; } set { this.entity.dto["Address2"] = value; } }
        public string District { get { return (string)this.entity.dto["District"]; } set { this.entity.dto["District"] = value; } }
        public short CityId { get { return (short)(long)this.entity.dto["CityId"]; } set { this.entity.dto["CityId"] = value; } }
        public string PostalCode { get { return (string)this.entity.dto["PostalCode"]; } set { this.entity.dto["PostalCode"] = value; } }
        public string Phone { get { return (string)this.entity.dto["Phone"]; } set { this.entity.dto["Phone"] = value; } }
        public object Location { get { return (object)this.entity.dto["Location"]; } set { this.entity.dto["Location"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public City City { get { return this.entity.NavigateSingle<City>("Address", "City"); } }
        [JsonIgnore]
        public IEnumerable<Customer> Customers { get { return this.entity.NavigateMulti<Customer>("Address", "Customers"); } }
        [JsonIgnore]
        public IEnumerable<Staff> Staffs { get { return this.entity.NavigateMulti<Staff>("Address", "Staffs"); } }
        [JsonIgnore]
        public IEnumerable<Store> Stores { get { return this.entity.NavigateMulti<Store>("Address", "Stores"); } }
        
    }

    public sealed class Category : IDerivedEntity
    {
        public Category(Entity entity)
        {
            if (entity.entityTypeName != "Category") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public sbyte CategoryId { get { return (sbyte)(long)this.entity.dto["CategoryId"]; } set { this.entity.dto["CategoryId"] = value; } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<FilmCategory> FilmCategories { get { return this.entity.NavigateMulti<FilmCategory>("Category", "FilmCategories"); } }
        
    }

    public sealed class City : IDerivedEntity
    {
        public City(Entity entity)
        {
            if (entity.entityTypeName != "City") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short CityId { get { return (short)(long)this.entity.dto["CityId"]; } set { this.entity.dto["CityId"] = value; } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = value; } }
        public short CountryId { get { return (short)(long)this.entity.dto["CountryId"]; } set { this.entity.dto["CountryId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Address> Addresses { get { return this.entity.NavigateMulti<Address>("City", "Addresses"); } }
        [JsonIgnore]
        public Country Country { get { return this.entity.NavigateSingle<Country>("City", "Country"); } }
        
    }

    public sealed class Country : IDerivedEntity
    {
        public Country(Entity entity)
        {
            if (entity.entityTypeName != "Country") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short CountryId { get { return (short)(long)this.entity.dto["CountryId"]; } set { this.entity.dto["CountryId"] = value; } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<City> Cities { get { return this.entity.NavigateMulti<City>("Country", "Cities"); } }
        
    }

    public sealed class Customer : IDerivedEntity
    {
        public Customer(Entity entity)
        {
            if (entity.entityTypeName != "Customer") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short CustomerId { get { return (short)(long)this.entity.dto["CustomerId"]; } set { this.entity.dto["CustomerId"] = value; } }
        public sbyte StoreId { get { return (sbyte)(long)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = value; } }
        public string FirstName { get { return (string)this.entity.dto["FirstName"]; } set { this.entity.dto["FirstName"] = value; } }
        public string LastName { get { return (string)this.entity.dto["LastName"]; } set { this.entity.dto["LastName"] = value; } }
        public string Email { get { return (string)this.entity.dto["Email"]; } set { this.entity.dto["Email"] = value; } }
        public short AddressId { get { return (short)(long)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = value; } }
        public bool Active { get { return (bool)this.entity.dto["Active"]; } set { this.entity.dto["Active"] = value; } }
        public DateTime CreateDate { get { return (DateTime)this.entity.dto["CreateDate"]; } set { this.entity.dto["CreateDate"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Store Store { get { return this.entity.NavigateSingle<Store>("Customer", "Store"); } }
        [JsonIgnore]
        public Address Address { get { return this.entity.NavigateSingle<Address>("Customer", "Address"); } }
        [JsonIgnore]
        public IEnumerable<Payment> Payments { get { return this.entity.NavigateMulti<Payment>("Customer", "Payments"); } }
        [JsonIgnore]
        public IEnumerable<Rental> Rentals { get { return this.entity.NavigateMulti<Rental>("Customer", "Rentals"); } }
        
    }

    public sealed class Film : IDerivedEntity
    {
        public Film(Entity entity)
        {
            if (entity.entityTypeName != "Film") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short FilmId { get { return (short)(long)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = value; } }
        public string Title { get { return (string)this.entity.dto["Title"]; } set { this.entity.dto["Title"] = value; } }
        public string Description { get { return (string)this.entity.dto["Description"]; } set { this.entity.dto["Description"] = value; } }
        public ushort? ReleaseYear { get { return (ushort?)(long)this.entity.dto["ReleaseYear"]; } set { this.entity.dto["ReleaseYear"] = value; } }
        public sbyte LanguageId { get { return (sbyte)(long)this.entity.dto["LanguageId"]; } set { this.entity.dto["LanguageId"] = value; } }
        public sbyte? OriginalLanguageId { get { return (sbyte?)(long)this.entity.dto["OriginalLanguageId"]; } set { this.entity.dto["OriginalLanguageId"] = value; } }
        public sbyte RentalDuration { get { return (sbyte)(long)this.entity.dto["RentalDuration"]; } set { this.entity.dto["RentalDuration"] = value; } }
        public float RentalRate { get { return (float)this.entity.dto["RentalRate"]; } set { this.entity.dto["RentalRate"] = value; } }
        public short? Length { get { return (short?)(long)this.entity.dto["Length"]; } set { this.entity.dto["Length"] = value; } }
        public float ReplacementCost { get { return (float)this.entity.dto["ReplacementCost"]; } set { this.entity.dto["ReplacementCost"] = value; } }
        public string Rating { get { return (string)this.entity.dto["Rating"]; } set { this.entity.dto["Rating"] = value; } }
        public string SpecialFeatures { get { return (string)this.entity.dto["SpecialFeatures"]; } set { this.entity.dto["SpecialFeatures"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Language Language { get { return this.entity.NavigateSingle<Language>("Film", "Language"); } }
        [JsonIgnore]
        public Language Language1 { get { return this.entity.NavigateSingle<Language>("Film", "Language1"); } }
        [JsonIgnore]
        public IEnumerable<FilmActor> FilmActors { get { return this.entity.NavigateMulti<FilmActor>("Film", "FilmActors"); } }
        [JsonIgnore]
        public IEnumerable<FilmCategory> FilmCategories { get { return this.entity.NavigateMulti<FilmCategory>("Film", "FilmCategories"); } }
        [JsonIgnore]
        public IEnumerable<Inventory> Inventories { get { return this.entity.NavigateMulti<Inventory>("Film", "Inventories"); } }
        
    }

    public sealed class FilmActor : IDerivedEntity
    {
        public FilmActor(Entity entity)
        {
            if (entity.entityTypeName != "FilmActor") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short ActorId { get { return (short)(long)this.entity.dto["ActorId"]; } set { this.entity.dto["ActorId"] = value; } }
        public short FilmId { get { return (short)(long)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Actor Actor { get { return this.entity.NavigateSingle<Actor>("FilmActor", "Actor"); } }
        [JsonIgnore]
        public Film Film { get { return this.entity.NavigateSingle<Film>("FilmActor", "Film"); } }
        
    }

    public sealed class FilmCategory : IDerivedEntity
    {
        public FilmCategory(Entity entity)
        {
            if (entity.entityTypeName != "FilmCategory") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short FilmId { get { return (short)(long)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = value; } }
        public sbyte CategoryId { get { return (sbyte)(long)this.entity.dto["CategoryId"]; } set { this.entity.dto["CategoryId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Film Film { get { return this.entity.NavigateSingle<Film>("FilmCategory", "Film"); } }
        [JsonIgnore]
        public Category Category { get { return this.entity.NavigateSingle<Category>("FilmCategory", "Category"); } }
        
    }

    public sealed class FilmText : IDerivedEntity
    {
        public FilmText(Entity entity)
        {
            if (entity.entityTypeName != "FilmText") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short FilmId { get { return (short)(long)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = value; } }
        public string Title { get { return (string)this.entity.dto["Title"]; } set { this.entity.dto["Title"] = value; } }
        public string Description { get { return (string)this.entity.dto["Description"]; } set { this.entity.dto["Description"] = value; } }
        
        
    }

    public sealed class Inventory : IDerivedEntity
    {
        public Inventory(Entity entity)
        {
            if (entity.entityTypeName != "Inventory") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public int InventoryId { get { return (int)(long)this.entity.dto["InventoryId"]; } set { this.entity.dto["InventoryId"] = value; } }
        public short FilmId { get { return (short)(long)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = value; } }
        public sbyte StoreId { get { return (sbyte)(long)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Film Film { get { return this.entity.NavigateSingle<Film>("Inventory", "Film"); } }
        [JsonIgnore]
        public Store Store { get { return this.entity.NavigateSingle<Store>("Inventory", "Store"); } }
        [JsonIgnore]
        public IEnumerable<Rental> Rentals { get { return this.entity.NavigateMulti<Rental>("Inventory", "Rentals"); } }
        
    }

    public sealed class Language : IDerivedEntity
    {
        public Language(Entity entity)
        {
            if (entity.entityTypeName != "Language") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public sbyte LanguageId { get { return (sbyte)(long)this.entity.dto["LanguageId"]; } set { this.entity.dto["LanguageId"] = value; } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Film> Films { get { return this.entity.NavigateMulti<Film>("Language", "Films"); } }
        [JsonIgnore]
        public IEnumerable<Film> Films1 { get { return this.entity.NavigateMulti<Film>("Language", "Films1"); } }
        
    }

    public sealed class Payment : IDerivedEntity
    {
        public Payment(Entity entity)
        {
            if (entity.entityTypeName != "Payment") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public short PaymentId { get { return (short)(long)this.entity.dto["PaymentId"]; } set { this.entity.dto["PaymentId"] = value; } }
        public short CustomerId { get { return (short)(long)this.entity.dto["CustomerId"]; } set { this.entity.dto["CustomerId"] = value; } }
        public sbyte StaffId { get { return (sbyte)(long)this.entity.dto["StaffId"]; } set { this.entity.dto["StaffId"] = value; } }
        public int? RentalId { get { return (int?)(long)this.entity.dto["RentalId"]; } set { this.entity.dto["RentalId"] = value; } }
        public float Amount { get { return (float)this.entity.dto["Amount"]; } set { this.entity.dto["Amount"] = value; } }
        public DateTime PaymentDate { get { return (DateTime)this.entity.dto["PaymentDate"]; } set { this.entity.dto["PaymentDate"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Customer Customer { get { return this.entity.NavigateSingle<Customer>("Payment", "Customer"); } }
        [JsonIgnore]
        public Staff Staff { get { return this.entity.NavigateSingle<Staff>("Payment", "Staff"); } }
        [JsonIgnore]
        public Rental Rental { get { return this.entity.NavigateSingle<Rental>("Payment", "Rental"); } }
        
    }

    public sealed class Rental : IDerivedEntity
    {
        public Rental(Entity entity)
        {
            if (entity.entityTypeName != "Rental") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public int RentalId { get { return (int)(long)this.entity.dto["RentalId"]; } set { this.entity.dto["RentalId"] = value; } }
        public DateTime RentalDate { get { return (DateTime)this.entity.dto["RentalDate"]; } set { this.entity.dto["RentalDate"] = value; } }
        public int InventoryId { get { return (int)(long)this.entity.dto["InventoryId"]; } set { this.entity.dto["InventoryId"] = value; } }
        public short CustomerId { get { return (short)(long)this.entity.dto["CustomerId"]; } set { this.entity.dto["CustomerId"] = value; } }
        public DateTime? ReturnDate { get { return (DateTime?)this.entity.dto["ReturnDate"]; } set { this.entity.dto["ReturnDate"] = value; } }
        public sbyte StaffId { get { return (sbyte)(long)this.entity.dto["StaffId"]; } set { this.entity.dto["StaffId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Payment> Payments { get { return this.entity.NavigateMulti<Payment>("Rental", "Payments"); } }
        [JsonIgnore]
        public Inventory Inventory { get { return this.entity.NavigateSingle<Inventory>("Rental", "Inventory"); } }
        [JsonIgnore]
        public Customer Customer { get { return this.entity.NavigateSingle<Customer>("Rental", "Customer"); } }
        [JsonIgnore]
        public Staff Staff { get { return this.entity.NavigateSingle<Staff>("Rental", "Staff"); } }
        
    }

    public sealed class Staff : IDerivedEntity
    {
        public Staff(Entity entity)
        {
            if (entity.entityTypeName != "Staff") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public sbyte StaffId { get { return (sbyte)(long)this.entity.dto["StaffId"]; } set { this.entity.dto["StaffId"] = value; } }
        public string FirstName { get { return (string)this.entity.dto["FirstName"]; } set { this.entity.dto["FirstName"] = value; } }
        public string LastName { get { return (string)this.entity.dto["LastName"]; } set { this.entity.dto["LastName"] = value; } }
        public short AddressId { get { return (short)(long)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = value; } }
        public byte[] Picture { get { return (byte[])this.entity.dto["Picture"]; } set { this.entity.dto["Picture"] = value; } }
        public string Email { get { return (string)this.entity.dto["Email"]; } set { this.entity.dto["Email"] = value; } }
        public sbyte StoreId { get { return (sbyte)(long)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = value; } }
        public bool Active { get { return (bool)this.entity.dto["Active"]; } set { this.entity.dto["Active"] = value; } }
        public string Username { get { return (string)this.entity.dto["Username"]; } set { this.entity.dto["Username"] = value; } }
        public string Password { get { return (string)this.entity.dto["Password"]; } set { this.entity.dto["Password"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Payment> Payments { get { return this.entity.NavigateMulti<Payment>("Staff", "Payments"); } }
        [JsonIgnore]
        public IEnumerable<Rental> Rentals { get { return this.entity.NavigateMulti<Rental>("Staff", "Rentals"); } }
        [JsonIgnore]
        public Address Address { get { return this.entity.NavigateSingle<Address>("Staff", "Address"); } }
        [JsonIgnore]
        public Store Store { get { return this.entity.NavigateSingle<Store>("Staff", "Store"); } }
        [JsonIgnore]
        public IEnumerable<Store> Stores { get { return this.entity.NavigateMulti<Store>("Staff", "Stores"); } }
        
    }

    public sealed class Store : IDerivedEntity
    {
        public Store(Entity entity)
        {
            if (entity.entityTypeName != "Store") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public sbyte StoreId { get { return (sbyte)(long)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = value; } }
        public sbyte ManagerStaffId { get { return (sbyte)(long)this.entity.dto["ManagerStaffId"]; } set { this.entity.dto["ManagerStaffId"] = value; } }
        public short AddressId { get { return (short)(long)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Customer> Customers { get { return this.entity.NavigateMulti<Customer>("Store", "Customers"); } }
        [JsonIgnore]
        public IEnumerable<Inventory> Inventories { get { return this.entity.NavigateMulti<Inventory>("Store", "Inventories"); } }
        [JsonIgnore]
        public IEnumerable<Staff> Staffs { get { return this.entity.NavigateMulti<Staff>("Store", "Staffs"); } }
        [JsonIgnore]
        public Staff Staff { get { return this.entity.NavigateSingle<Staff>("Store", "Staff"); } }
        [JsonIgnore]
        public Address Address { get { return this.entity.NavigateSingle<Address>("Store", "Address"); } }
        
    }

}

#pragma warning restore SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300
