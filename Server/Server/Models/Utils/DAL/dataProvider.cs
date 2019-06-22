#pragma warning disable SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300

//------------------------------------------------------------------------------
//    This code was auto-generated.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//------------------------------------------------------------------------------

using Newtonsoft.Json;
using Server.Models.Utils.DAL.Common;
using System;
using System.Collections.Generic;

namespace Server.Models.Utils.DAL
{
    public class DataService : DataServiceEntity<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>
    {
        public DataService(string metadataFileName = "", string connectionString = "") : base(metadataFileName, connectionString)
        {
            this.From = new ServiceLocation<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>()
            {
                Local = new ViewType<LocalEntityViews, LocalDtoViews>() { EntityView = new LocalEntityViews(this.DataContext), DtoView = new LocalDtoViews(this.DataContext, this.Metadata) },
                Remote = new ViewType<RemoteEntityViews, RemoteDtoViews>() { EntityView = new RemoteEntityViews(this.DataAdapter, this.DataContext), DtoView = new RemoteDtoViews(this.DataAdapter) }
            };
        }
    }

    public class LocalEntityViews : PropertyList
    {
        public LocalEntityViews(DataContext dataContext) : base(dataContext) { }
        
        public DataViewLocalEntity<Actor> Actors { get { return this.GetPropertyValue<DataViewLocalEntity<Actor>>(); } }
        public DataViewLocalEntity<Address> Addresses { get { return this.GetPropertyValue<DataViewLocalEntity<Address>>(); } }
        public DataViewLocalEntity<Category> Categories { get { return this.GetPropertyValue<DataViewLocalEntity<Category>>(); } }
        public DataViewLocalEntity<City> Cities { get { return this.GetPropertyValue<DataViewLocalEntity<City>>(); } }
        public DataViewLocalEntity<Country> Countries { get { return this.GetPropertyValue<DataViewLocalEntity<Country>>(); } }
        public DataViewLocalEntity<Customer> Customers { get { return this.GetPropertyValue<DataViewLocalEntity<Customer>>(); } }
        public DataViewLocalEntity<Film> Films { get { return this.GetPropertyValue<DataViewLocalEntity<Film>>(); } }
        public DataViewLocalEntity<FilmActor> FilmActors { get { return this.GetPropertyValue<DataViewLocalEntity<FilmActor>>(); } }
        public DataViewLocalEntity<FilmCategory> FilmCategories { get { return this.GetPropertyValue<DataViewLocalEntity<FilmCategory>>(); } }
        public DataViewLocalEntity<FilmText> FilmTexts { get { return this.GetPropertyValue<DataViewLocalEntity<FilmText>>(); } }
        public DataViewLocalEntity<Inventory> Inventories { get { return this.GetPropertyValue<DataViewLocalEntity<Inventory>>(); } }
        public DataViewLocalEntity<Language> Languages { get { return this.GetPropertyValue<DataViewLocalEntity<Language>>(); } }
        public DataViewLocalEntity<Payment> Payments { get { return this.GetPropertyValue<DataViewLocalEntity<Payment>>(); } }
        public DataViewLocalEntity<Rental> Rentals { get { return this.GetPropertyValue<DataViewLocalEntity<Rental>>(); } }
        public DataViewLocalEntity<Staff> Staffs { get { return this.GetPropertyValue<DataViewLocalEntity<Staff>>(); } }
        public DataViewLocalEntity<Store> Stores { get { return this.GetPropertyValue<DataViewLocalEntity<Store>>(); } }
    }

    public class RemoteEntityViews : PropertyList
    {
        public RemoteEntityViews(DataAdapter dataAdapter, DataContext dataContext) : base(dataAdapter, dataContext) { }
        
        public DataViewRemoteEntity<Actor> Actors { get { return this.GetPropertyValue<DataViewRemoteEntity<Actor>>(); } }
        public DataViewRemoteEntity<Address> Addresses { get { return this.GetPropertyValue<DataViewRemoteEntity<Address>>(); } }
        public DataViewRemoteEntity<Category> Categories { get { return this.GetPropertyValue<DataViewRemoteEntity<Category>>(); } }
        public DataViewRemoteEntity<City> Cities { get { return this.GetPropertyValue<DataViewRemoteEntity<City>>(); } }
        public DataViewRemoteEntity<Country> Countries { get { return this.GetPropertyValue<DataViewRemoteEntity<Country>>(); } }
        public DataViewRemoteEntity<Customer> Customers { get { return this.GetPropertyValue<DataViewRemoteEntity<Customer>>(); } }
        public DataViewRemoteEntity<Film> Films { get { return this.GetPropertyValue<DataViewRemoteEntity<Film>>(); } }
        public DataViewRemoteEntity<FilmActor> FilmActors { get { return this.GetPropertyValue<DataViewRemoteEntity<FilmActor>>(); } }
        public DataViewRemoteEntity<FilmCategory> FilmCategories { get { return this.GetPropertyValue<DataViewRemoteEntity<FilmCategory>>(); } }
        public DataViewRemoteEntity<FilmText> FilmTexts { get { return this.GetPropertyValue<DataViewRemoteEntity<FilmText>>(); } }
        public DataViewRemoteEntity<Inventory> Inventories { get { return this.GetPropertyValue<DataViewRemoteEntity<Inventory>>(); } }
        public DataViewRemoteEntity<Language> Languages { get { return this.GetPropertyValue<DataViewRemoteEntity<Language>>(); } }
        public DataViewRemoteEntity<Payment> Payments { get { return this.GetPropertyValue<DataViewRemoteEntity<Payment>>(); } }
        public DataViewRemoteEntity<Rental> Rentals { get { return this.GetPropertyValue<DataViewRemoteEntity<Rental>>(); } }
        public DataViewRemoteEntity<Staff> Staffs { get { return this.GetPropertyValue<DataViewRemoteEntity<Staff>>(); } }
        public DataViewRemoteEntity<Store> Stores { get { return this.GetPropertyValue<DataViewRemoteEntity<Store>>(); } }
    }

    public class LocalDtoViews : PropertyList
    {
        public LocalDtoViews(DataContext dataContext, Metadata metadata) : base(dataContext, metadata) { }
        
        public DataViewLocalDto<Actor> Actors { get { return this.GetPropertyValue<DataViewLocalDto<Actor>>(); } }
        public DataViewLocalDto<Address> Addresses { get { return this.GetPropertyValue<DataViewLocalDto<Address>>(); } }
        public DataViewLocalDto<Category> Categories { get { return this.GetPropertyValue<DataViewLocalDto<Category>>(); } }
        public DataViewLocalDto<City> Cities { get { return this.GetPropertyValue<DataViewLocalDto<City>>(); } }
        public DataViewLocalDto<Country> Countries { get { return this.GetPropertyValue<DataViewLocalDto<Country>>(); } }
        public DataViewLocalDto<Customer> Customers { get { return this.GetPropertyValue<DataViewLocalDto<Customer>>(); } }
        public DataViewLocalDto<Film> Films { get { return this.GetPropertyValue<DataViewLocalDto<Film>>(); } }
        public DataViewLocalDto<FilmActor> FilmActors { get { return this.GetPropertyValue<DataViewLocalDto<FilmActor>>(); } }
        public DataViewLocalDto<FilmCategory> FilmCategories { get { return this.GetPropertyValue<DataViewLocalDto<FilmCategory>>(); } }
        public DataViewLocalDto<FilmText> FilmTexts { get { return this.GetPropertyValue<DataViewLocalDto<FilmText>>(); } }
        public DataViewLocalDto<Inventory> Inventories { get { return this.GetPropertyValue<DataViewLocalDto<Inventory>>(); } }
        public DataViewLocalDto<Language> Languages { get { return this.GetPropertyValue<DataViewLocalDto<Language>>(); } }
        public DataViewLocalDto<Payment> Payments { get { return this.GetPropertyValue<DataViewLocalDto<Payment>>(); } }
        public DataViewLocalDto<Rental> Rentals { get { return this.GetPropertyValue<DataViewLocalDto<Rental>>(); } }
        public DataViewLocalDto<Staff> Staffs { get { return this.GetPropertyValue<DataViewLocalDto<Staff>>(); } }
        public DataViewLocalDto<Store> Stores { get { return this.GetPropertyValue<DataViewLocalDto<Store>>(); } }
    }

    public class RemoteDtoViews : PropertyList
    {
        public RemoteDtoViews(DataAdapter dataAdapter) : base(dataAdapter) { }
        
        public DataViewRemoteDto<Actor> Actors { get { return this.GetPropertyValue<DataViewRemoteDto<Actor>>(); } }
        public DataViewRemoteDto<Address> Addresses { get { return this.GetPropertyValue<DataViewRemoteDto<Address>>(); } }
        public DataViewRemoteDto<Category> Categories { get { return this.GetPropertyValue<DataViewRemoteDto<Category>>(); } }
        public DataViewRemoteDto<City> Cities { get { return this.GetPropertyValue<DataViewRemoteDto<City>>(); } }
        public DataViewRemoteDto<Country> Countries { get { return this.GetPropertyValue<DataViewRemoteDto<Country>>(); } }
        public DataViewRemoteDto<Customer> Customers { get { return this.GetPropertyValue<DataViewRemoteDto<Customer>>(); } }
        public DataViewRemoteDto<Film> Films { get { return this.GetPropertyValue<DataViewRemoteDto<Film>>(); } }
        public DataViewRemoteDto<FilmActor> FilmActors { get { return this.GetPropertyValue<DataViewRemoteDto<FilmActor>>(); } }
        public DataViewRemoteDto<FilmCategory> FilmCategories { get { return this.GetPropertyValue<DataViewRemoteDto<FilmCategory>>(); } }
        public DataViewRemoteDto<FilmText> FilmTexts { get { return this.GetPropertyValue<DataViewRemoteDto<FilmText>>(); } }
        public DataViewRemoteDto<Inventory> Inventories { get { return this.GetPropertyValue<DataViewRemoteDto<Inventory>>(); } }
        public DataViewRemoteDto<Language> Languages { get { return this.GetPropertyValue<DataViewRemoteDto<Language>>(); } }
        public DataViewRemoteDto<Payment> Payments { get { return this.GetPropertyValue<DataViewRemoteDto<Payment>>(); } }
        public DataViewRemoteDto<Rental> Rentals { get { return this.GetPropertyValue<DataViewRemoteDto<Rental>>(); } }
        public DataViewRemoteDto<Staff> Staffs { get { return this.GetPropertyValue<DataViewRemoteDto<Staff>>(); } }
        public DataViewRemoteDto<Store> Stores { get { return this.GetPropertyValue<DataViewRemoteDto<Store>>(); } }
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

        public short ActorId { get { return (short)this["ActorId"]; } set { this["ActorId"] = value; } }
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

        public short AddressId { get { return (short)this["AddressId"]; } set { this["AddressId"] = value; } }
        public string Address1 { get { return (string)this["Address1"]; } set { this["Address1"] = value; } }
        public string Address2 { get { return (string)this["Address2"]; } set { this["Address2"] = value; } }
        public string District { get { return (string)this["District"]; } set { this["District"] = value; } }
        public short CityId { get { return (short)this["CityId"]; } set { this["CityId"] = value; } }
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

        public sbyte CategoryId { get { return (sbyte)this["CategoryId"]; } set { this["CategoryId"] = value; } }
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

        public short CityId { get { return (short)this["CityId"]; } set { this["CityId"] = value; } }
        public string Name { get { return (string)this["Name"]; } set { this["Name"] = value; } }
        public short CountryId { get { return (short)this["CountryId"]; } set { this["CountryId"] = value; } }
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

        public short CountryId { get { return (short)this["CountryId"]; } set { this["CountryId"] = value; } }
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

        public short CustomerId { get { return (short)this["CustomerId"]; } set { this["CustomerId"] = value; } }
        public sbyte StoreId { get { return (sbyte)this["StoreId"]; } set { this["StoreId"] = value; } }
        public string FirstName { get { return (string)this["FirstName"]; } set { this["FirstName"] = value; } }
        public string LastName { get { return (string)this["LastName"]; } set { this["LastName"] = value; } }
        public string Email { get { return (string)this["Email"]; } set { this["Email"] = value; } }
        public short AddressId { get { return (short)this["AddressId"]; } set { this["AddressId"] = value; } }
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

        public short FilmId { get { return (short)this["FilmId"]; } set { this["FilmId"] = value; } }
        public string Title { get { return (string)this["Title"]; } set { this["Title"] = value; } }
        public string Description { get { return (string)this["Description"]; } set { this["Description"] = value; } }
        public ushort? ReleaseYear { get { return (ushort?)this["ReleaseYear"]; } set { this["ReleaseYear"] = value; } }
        public sbyte LanguageId { get { return (sbyte)this["LanguageId"]; } set { this["LanguageId"] = value; } }
        public sbyte? OriginalLanguageId { get { return (sbyte?)this["OriginalLanguageId"]; } set { this["OriginalLanguageId"] = value; } }
        public sbyte RentalDuration { get { return (sbyte)this["RentalDuration"]; } set { this["RentalDuration"] = value; } }
        public float RentalRate { get { return (float)this["RentalRate"]; } set { this["RentalRate"] = value; } }
        public short? Length { get { return (short?)this["Length"]; } set { this["Length"] = value; } }
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

        public short ActorId { get { return (short)this["ActorId"]; } set { this["ActorId"] = value; } }
        public short FilmId { get { return (short)this["FilmId"]; } set { this["FilmId"] = value; } }
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

        public short FilmId { get { return (short)this["FilmId"]; } set { this["FilmId"] = value; } }
        public sbyte CategoryId { get { return (sbyte)this["CategoryId"]; } set { this["CategoryId"] = value; } }
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

        public short FilmId { get { return (short)this["FilmId"]; } set { this["FilmId"] = value; } }
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

        public int InventoryId { get { return (int)this["InventoryId"]; } set { this["InventoryId"] = value; } }
        public short FilmId { get { return (short)this["FilmId"]; } set { this["FilmId"] = value; } }
        public sbyte StoreId { get { return (sbyte)this["StoreId"]; } set { this["StoreId"] = value; } }
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

        public sbyte LanguageId { get { return (sbyte)this["LanguageId"]; } set { this["LanguageId"] = value; } }
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

        public short PaymentId { get { return (short)this["PaymentId"]; } set { this["PaymentId"] = value; } }
        public short CustomerId { get { return (short)this["CustomerId"]; } set { this["CustomerId"] = value; } }
        public sbyte StaffId { get { return (sbyte)this["StaffId"]; } set { this["StaffId"] = value; } }
        public int? RentalId { get { return (int?)this["RentalId"]; } set { this["RentalId"] = value; } }
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

        public int RentalId { get { return (int)this["RentalId"]; } set { this["RentalId"] = value; } }
        public DateTime RentalDate { get { return (DateTime)this["RentalDate"]; } set { this["RentalDate"] = value; } }
        public int InventoryId { get { return (int)this["InventoryId"]; } set { this["InventoryId"] = value; } }
        public short CustomerId { get { return (short)this["CustomerId"]; } set { this["CustomerId"] = value; } }
        public DateTime? ReturnDate { get { return (DateTime?)this["ReturnDate"]; } set { this["ReturnDate"] = value; } }
        public sbyte StaffId { get { return (sbyte)this["StaffId"]; } set { this["StaffId"] = value; } }
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

        public sbyte StaffId { get { return (sbyte)this["StaffId"]; } set { this["StaffId"] = value; } }
        public string FirstName { get { return (string)this["FirstName"]; } set { this["FirstName"] = value; } }
        public string LastName { get { return (string)this["LastName"]; } set { this["LastName"] = value; } }
        public short AddressId { get { return (short)this["AddressId"]; } set { this["AddressId"] = value; } }
        public byte[] Picture { get { return (byte[])this["Picture"]; } set { this["Picture"] = value; } }
        public string Email { get { return (string)this["Email"]; } set { this["Email"] = value; } }
        public sbyte StoreId { get { return (sbyte)this["StoreId"]; } set { this["StoreId"] = value; } }
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

        public sbyte StoreId { get { return (sbyte)this["StoreId"]; } set { this["StoreId"] = value; } }
        public sbyte ManagerStaffId { get { return (sbyte)this["ManagerStaffId"]; } set { this["ManagerStaffId"] = value; } }
        public short AddressId { get { return (short)this["AddressId"]; } set { this["AddressId"] = value; } }
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
