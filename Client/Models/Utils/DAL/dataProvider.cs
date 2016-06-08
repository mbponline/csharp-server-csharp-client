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

    public sealed class Actor : Entity
    {
        public Actor() : base()
        {
            this.ActorId = 0;
            this.FirstName = "";
            this.LastName = "";
            this.LastUpdate = DateTime.Now;
        }

        public short ActorId { get { return (short)(long)this["ActorId"]; } set { this["ActorId"] = value; } }
        public string FirstName { get { return (string)this["FirstName"]; } set { this["FirstName"] = value; } }
        public string LastName { get { return (string)this["LastName"]; } set { this["LastName"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<FilmActor> FilmActors { get { return this.NavigateMulti<FilmActor>("Actor", "FilmActors"); } }
        
    }

    public sealed class Address : Entity
    {
        public Address() : base()
        {
            this.AddressId = 0;
            this.Address1 = "";
            this.Address2 = null;
            this.District = "";
            this.CityId = 0;
            this.PostalCode = null;
            this.Phone = "";
            this.Location = new { lat = 0, lon = 0 };
            this.LastUpdate = DateTime.Now;
        }

        public short AddressId { get { return (short)(long)this["AddressId"]; } set { this["AddressId"] = value; } }
        public string Address1 { get { return (string)this["Address1"]; } set { this["Address1"] = value; } }
        public string Address2 { get { return (string)this["Address2"]; } set { this["Address2"] = value; } }
        public string District { get { return (string)this["District"]; } set { this["District"] = value; } }
        public short CityId { get { return (short)(long)this["CityId"]; } set { this["CityId"] = value; } }
        public string PostalCode { get { return (string)this["PostalCode"]; } set { this["PostalCode"] = value; } }
        public string Phone { get { return (string)this["Phone"]; } set { this["Phone"] = value; } }
        public object Location { get { return (object)this["Location"]; } set { this["Location"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public City City { get { return this.NavigateSingle<City>("Address", "City"); } }
        [JsonIgnore]
        public IEnumerable<Customer> Customers { get { return this.NavigateMulti<Customer>("Address", "Customers"); } }
        [JsonIgnore]
        public IEnumerable<Staff> Staffs { get { return this.NavigateMulti<Staff>("Address", "Staffs"); } }
        [JsonIgnore]
        public IEnumerable<Store> Stores { get { return this.NavigateMulti<Store>("Address", "Stores"); } }
        
    }

    public sealed class Category : Entity
    {
        public Category() : base()
        {
            this.CategoryId = 0;
            this.Name = "";
            this.LastUpdate = DateTime.Now;
        }

        public sbyte CategoryId { get { return (sbyte)(long)this["CategoryId"]; } set { this["CategoryId"] = value; } }
        public string Name { get { return (string)this["Name"]; } set { this["Name"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<FilmCategory> FilmCategories { get { return this.NavigateMulti<FilmCategory>("Category", "FilmCategories"); } }
        
    }

    public sealed class City : Entity
    {
        public City() : base()
        {
            this.CityId = 0;
            this.Name = "";
            this.CountryId = 0;
            this.LastUpdate = DateTime.Now;
        }

        public short CityId { get { return (short)(long)this["CityId"]; } set { this["CityId"] = value; } }
        public string Name { get { return (string)this["Name"]; } set { this["Name"] = value; } }
        public short CountryId { get { return (short)(long)this["CountryId"]; } set { this["CountryId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Address> Addresses { get { return this.NavigateMulti<Address>("City", "Addresses"); } }
        [JsonIgnore]
        public Country Country { get { return this.NavigateSingle<Country>("City", "Country"); } }
        
    }

    public sealed class Country : Entity
    {
        public Country() : base()
        {
            this.CountryId = 0;
            this.Name = "";
            this.LastUpdate = DateTime.Now;
        }

        public short CountryId { get { return (short)(long)this["CountryId"]; } set { this["CountryId"] = value; } }
        public string Name { get { return (string)this["Name"]; } set { this["Name"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<City> Cities { get { return this.NavigateMulti<City>("Country", "Cities"); } }
        
    }

    public sealed class Customer : Entity
    {
        public Customer() : base()
        {
            this.CustomerId = 0;
            this.StoreId = 0;
            this.FirstName = "";
            this.LastName = "";
            this.Email = null;
            this.AddressId = 0;
            this.Active = false;
            this.CreateDate = DateTime.Now;
            this.LastUpdate = DateTime.Now;
        }

        public short CustomerId { get { return (short)(long)this["CustomerId"]; } set { this["CustomerId"] = value; } }
        public sbyte StoreId { get { return (sbyte)(long)this["StoreId"]; } set { this["StoreId"] = value; } }
        public string FirstName { get { return (string)this["FirstName"]; } set { this["FirstName"] = value; } }
        public string LastName { get { return (string)this["LastName"]; } set { this["LastName"] = value; } }
        public string Email { get { return (string)this["Email"]; } set { this["Email"] = value; } }
        public short AddressId { get { return (short)(long)this["AddressId"]; } set { this["AddressId"] = value; } }
        public bool Active { get { return (bool)this["Active"]; } set { this["Active"] = value; } }
        public DateTime CreateDate { get { return (DateTime)this["CreateDate"]; } set { this["CreateDate"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Store Store { get { return this.NavigateSingle<Store>("Customer", "Store"); } }
        [JsonIgnore]
        public Address Address { get { return this.NavigateSingle<Address>("Customer", "Address"); } }
        [JsonIgnore]
        public IEnumerable<Payment> Payments { get { return this.NavigateMulti<Payment>("Customer", "Payments"); } }
        [JsonIgnore]
        public IEnumerable<Rental> Rentals { get { return this.NavigateMulti<Rental>("Customer", "Rentals"); } }
        
    }

    public sealed class Film : Entity
    {
        public Film() : base()
        {
            this.FilmId = 0;
            this.Title = "";
            this.Description = null;
            this.ReleaseYear = null;
            this.LanguageId = 0;
            this.OriginalLanguageId = null;
            this.RentalDuration = 0;
            this.RentalRate = 0;
            this.Length = null;
            this.ReplacementCost = 0;
            this.Rating = null;
            this.SpecialFeatures = null;
            this.LastUpdate = DateTime.Now;
        }

        public short FilmId { get { return (short)(long)this["FilmId"]; } set { this["FilmId"] = value; } }
        public string Title { get { return (string)this["Title"]; } set { this["Title"] = value; } }
        public string Description { get { return (string)this["Description"]; } set { this["Description"] = value; } }
        public ushort? ReleaseYear { get { return (ushort?)(long)this["ReleaseYear"]; } set { this["ReleaseYear"] = value; } }
        public sbyte LanguageId { get { return (sbyte)(long)this["LanguageId"]; } set { this["LanguageId"] = value; } }
        public sbyte? OriginalLanguageId { get { return (sbyte?)(long)this["OriginalLanguageId"]; } set { this["OriginalLanguageId"] = value; } }
        public sbyte RentalDuration { get { return (sbyte)(long)this["RentalDuration"]; } set { this["RentalDuration"] = value; } }
        public float RentalRate { get { return (float)this["RentalRate"]; } set { this["RentalRate"] = value; } }
        public short? Length { get { return (short?)(long)this["Length"]; } set { this["Length"] = value; } }
        public float ReplacementCost { get { return (float)this["ReplacementCost"]; } set { this["ReplacementCost"] = value; } }
        public string Rating { get { return (string)this["Rating"]; } set { this["Rating"] = value; } }
        public string SpecialFeatures { get { return (string)this["SpecialFeatures"]; } set { this["SpecialFeatures"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Language Language { get { return this.NavigateSingle<Language>("Film", "Language"); } }
        [JsonIgnore]
        public Language Language1 { get { return this.NavigateSingle<Language>("Film", "Language1"); } }
        [JsonIgnore]
        public IEnumerable<FilmActor> FilmActors { get { return this.NavigateMulti<FilmActor>("Film", "FilmActors"); } }
        [JsonIgnore]
        public IEnumerable<FilmCategory> FilmCategories { get { return this.NavigateMulti<FilmCategory>("Film", "FilmCategories"); } }
        [JsonIgnore]
        public IEnumerable<Inventory> Inventories { get { return this.NavigateMulti<Inventory>("Film", "Inventories"); } }
        
    }

    public sealed class FilmActor : Entity
    {
        public FilmActor() : base()
        {
            this.ActorId = 0;
            this.FilmId = 0;
            this.LastUpdate = DateTime.Now;
        }

        public short ActorId { get { return (short)(long)this["ActorId"]; } set { this["ActorId"] = value; } }
        public short FilmId { get { return (short)(long)this["FilmId"]; } set { this["FilmId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Actor Actor { get { return this.NavigateSingle<Actor>("FilmActor", "Actor"); } }
        [JsonIgnore]
        public Film Film { get { return this.NavigateSingle<Film>("FilmActor", "Film"); } }
        
    }

    public sealed class FilmCategory : Entity
    {
        public FilmCategory() : base()
        {
            this.FilmId = 0;
            this.CategoryId = 0;
            this.LastUpdate = DateTime.Now;
        }

        public short FilmId { get { return (short)(long)this["FilmId"]; } set { this["FilmId"] = value; } }
        public sbyte CategoryId { get { return (sbyte)(long)this["CategoryId"]; } set { this["CategoryId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Film Film { get { return this.NavigateSingle<Film>("FilmCategory", "Film"); } }
        [JsonIgnore]
        public Category Category { get { return this.NavigateSingle<Category>("FilmCategory", "Category"); } }
        
    }

    public sealed class FilmText : Entity
    {
        public FilmText() : base()
        {
            this.FilmId = 0;
            this.Title = "";
            this.Description = null;
        }

        public short FilmId { get { return (short)(long)this["FilmId"]; } set { this["FilmId"] = value; } }
        public string Title { get { return (string)this["Title"]; } set { this["Title"] = value; } }
        public string Description { get { return (string)this["Description"]; } set { this["Description"] = value; } }
        
        
    }

    public sealed class Inventory : Entity
    {
        public Inventory() : base()
        {
            this.InventoryId = 0;
            this.FilmId = 0;
            this.StoreId = 0;
            this.LastUpdate = DateTime.Now;
        }

        public int InventoryId { get { return (int)(long)this["InventoryId"]; } set { this["InventoryId"] = value; } }
        public short FilmId { get { return (short)(long)this["FilmId"]; } set { this["FilmId"] = value; } }
        public sbyte StoreId { get { return (sbyte)(long)this["StoreId"]; } set { this["StoreId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Film Film { get { return this.NavigateSingle<Film>("Inventory", "Film"); } }
        [JsonIgnore]
        public Store Store { get { return this.NavigateSingle<Store>("Inventory", "Store"); } }
        [JsonIgnore]
        public IEnumerable<Rental> Rentals { get { return this.NavigateMulti<Rental>("Inventory", "Rentals"); } }
        
    }

    public sealed class Language : Entity
    {
        public Language() : base()
        {
            this.LanguageId = 0;
            this.Name = "";
            this.LastUpdate = DateTime.Now;
        }

        public sbyte LanguageId { get { return (sbyte)(long)this["LanguageId"]; } set { this["LanguageId"] = value; } }
        public string Name { get { return (string)this["Name"]; } set { this["Name"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Film> Films { get { return this.NavigateMulti<Film>("Language", "Films"); } }
        [JsonIgnore]
        public IEnumerable<Film> Films1 { get { return this.NavigateMulti<Film>("Language", "Films1"); } }
        
    }

    public sealed class Payment : Entity
    {
        public Payment() : base()
        {
            this.PaymentId = 0;
            this.CustomerId = 0;
            this.StaffId = 0;
            this.RentalId = null;
            this.Amount = 0;
            this.PaymentDate = DateTime.Now;
            this.LastUpdate = DateTime.Now;
        }

        public short PaymentId { get { return (short)(long)this["PaymentId"]; } set { this["PaymentId"] = value; } }
        public short CustomerId { get { return (short)(long)this["CustomerId"]; } set { this["CustomerId"] = value; } }
        public sbyte StaffId { get { return (sbyte)(long)this["StaffId"]; } set { this["StaffId"] = value; } }
        public int? RentalId { get { return (int?)(long)this["RentalId"]; } set { this["RentalId"] = value; } }
        public float Amount { get { return (float)this["Amount"]; } set { this["Amount"] = value; } }
        public DateTime PaymentDate { get { return (DateTime)this["PaymentDate"]; } set { this["PaymentDate"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public Customer Customer { get { return this.NavigateSingle<Customer>("Payment", "Customer"); } }
        [JsonIgnore]
        public Staff Staff { get { return this.NavigateSingle<Staff>("Payment", "Staff"); } }
        [JsonIgnore]
        public Rental Rental { get { return this.NavigateSingle<Rental>("Payment", "Rental"); } }
        
    }

    public sealed class Rental : Entity
    {
        public Rental() : base()
        {
            this.RentalId = 0;
            this.RentalDate = DateTime.Now;
            this.InventoryId = 0;
            this.CustomerId = 0;
            this.ReturnDate = null;
            this.StaffId = 0;
            this.LastUpdate = DateTime.Now;
        }

        public int RentalId { get { return (int)(long)this["RentalId"]; } set { this["RentalId"] = value; } }
        public DateTime RentalDate { get { return (DateTime)this["RentalDate"]; } set { this["RentalDate"] = value; } }
        public int InventoryId { get { return (int)(long)this["InventoryId"]; } set { this["InventoryId"] = value; } }
        public short CustomerId { get { return (short)(long)this["CustomerId"]; } set { this["CustomerId"] = value; } }
        public DateTime? ReturnDate { get { return (DateTime?)this["ReturnDate"]; } set { this["ReturnDate"] = value; } }
        public sbyte StaffId { get { return (sbyte)(long)this["StaffId"]; } set { this["StaffId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Payment> Payments { get { return this.NavigateMulti<Payment>("Rental", "Payments"); } }
        [JsonIgnore]
        public Inventory Inventory { get { return this.NavigateSingle<Inventory>("Rental", "Inventory"); } }
        [JsonIgnore]
        public Customer Customer { get { return this.NavigateSingle<Customer>("Rental", "Customer"); } }
        [JsonIgnore]
        public Staff Staff { get { return this.NavigateSingle<Staff>("Rental", "Staff"); } }
        
    }

    public sealed class Staff : Entity
    {
        public Staff() : base()
        {
            this.StaffId = 0;
            this.FirstName = "";
            this.LastName = "";
            this.AddressId = 0;
            this.Picture = null;
            this.Email = null;
            this.StoreId = 0;
            this.Active = false;
            this.Username = "";
            this.Password = null;
            this.LastUpdate = DateTime.Now;
        }

        public sbyte StaffId { get { return (sbyte)(long)this["StaffId"]; } set { this["StaffId"] = value; } }
        public string FirstName { get { return (string)this["FirstName"]; } set { this["FirstName"] = value; } }
        public string LastName { get { return (string)this["LastName"]; } set { this["LastName"] = value; } }
        public short AddressId { get { return (short)(long)this["AddressId"]; } set { this["AddressId"] = value; } }
        public byte[] Picture { get { return (byte[])this["Picture"]; } set { this["Picture"] = value; } }
        public string Email { get { return (string)this["Email"]; } set { this["Email"] = value; } }
        public sbyte StoreId { get { return (sbyte)(long)this["StoreId"]; } set { this["StoreId"] = value; } }
        public bool Active { get { return (bool)this["Active"]; } set { this["Active"] = value; } }
        public string Username { get { return (string)this["Username"]; } set { this["Username"] = value; } }
        public string Password { get { return (string)this["Password"]; } set { this["Password"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Payment> Payments { get { return this.NavigateMulti<Payment>("Staff", "Payments"); } }
        [JsonIgnore]
        public IEnumerable<Rental> Rentals { get { return this.NavigateMulti<Rental>("Staff", "Rentals"); } }
        [JsonIgnore]
        public Address Address { get { return this.NavigateSingle<Address>("Staff", "Address"); } }
        [JsonIgnore]
        public Store Store { get { return this.NavigateSingle<Store>("Staff", "Store"); } }
        [JsonIgnore]
        public IEnumerable<Store> Stores { get { return this.NavigateMulti<Store>("Staff", "Stores"); } }
        
    }

    public sealed class Store : Entity
    {
        public Store() : base()
        {
            this.StoreId = 0;
            this.ManagerStaffId = 0;
            this.AddressId = 0;
            this.LastUpdate = DateTime.Now;
        }

        public sbyte StoreId { get { return (sbyte)(long)this["StoreId"]; } set { this["StoreId"] = value; } }
        public sbyte ManagerStaffId { get { return (sbyte)(long)this["ManagerStaffId"]; } set { this["ManagerStaffId"] = value; } }
        public short AddressId { get { return (short)(long)this["AddressId"]; } set { this["AddressId"] = value; } }
        public DateTime LastUpdate { get { return (DateTime)this["LastUpdate"]; } set { this["LastUpdate"] = value; } }
        
        [JsonIgnore]
        public IEnumerable<Customer> Customers { get { return this.NavigateMulti<Customer>("Store", "Customers"); } }
        [JsonIgnore]
        public IEnumerable<Inventory> Inventories { get { return this.NavigateMulti<Inventory>("Store", "Inventories"); } }
        [JsonIgnore]
        public IEnumerable<Staff> Staffs { get { return this.NavigateMulti<Staff>("Store", "Staffs"); } }
        [JsonIgnore]
        public Staff Staff { get { return this.NavigateSingle<Staff>("Store", "Staff"); } }
        [JsonIgnore]
        public Address Address { get { return this.NavigateSingle<Address>("Store", "Address"); } }
        
    }

}

#pragma warning restore SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300
