#pragma warning disable SA1649, SA1128, SA1005, SA1516, SA1402, SA1028, SA1119, SA1507, SA1502, SA1508, SA1122, SA1633, SA1300

//------------------------------------------------------------------------------
//    This code was auto-generated.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
//------------------------------------------------------------------------------

using NavyBlueDtos;
using NavyBlueEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using MetadataSrv = NavyBlueDtos.MetadataSrv;

namespace Server.Models.DataAccess
{
    public interface IDataProvider
    {
        DataService CreateDataServiceInstance();
    }

    public class DataProvider : IDataProvider
    {
        private readonly IDataProviderDto dataProviderDto;
        
        public DataProvider(IDataProviderDto dataProviderDto)
        {
            this.dataProviderDto = dataProviderDto;
        }

        public DataService CreateDataServiceInstance()
        {
            var dataServiceDto = this.dataProviderDto.CreateDataServiceInstance();
            var dataService = new DataService(dataServiceDto);
            return dataService;
        }
    }

    public class DataService : DataServiceEntity<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>
    {
        public DataService(DataServiceDto dataServiceDto) : base(dataServiceDto)
        {
            this.From = new ServiceLocation<LocalEntityViews, LocalDtoViews, RemoteEntityViews, RemoteDtoViews>()
            {
                Local = new ViewType<LocalEntityViews, LocalDtoViews>() { EntityView = new LocalEntityViews(this.DataContext), DtoView = new LocalDtoViews(this.DataContext, dataServiceDto.MetadataSrv) },
                Remote = new ViewType<RemoteEntityViews, RemoteDtoViews>() { EntityView = new RemoteEntityViews(dataServiceDto.DataViewDto, this.DataContext), DtoView = new RemoteDtoViews(dataServiceDto.DataViewDto) }
            };
        }
    }

    public class LocalEntityViews : LocalEntityViewsBase
    {
        public LocalEntityViews(DataContext dataContext) : base(dataContext)
        {
            //this.["Actors"] = new DataViewLocalEntity<Actor>(dataContext);
            //this.["Addresses"] = new DataViewLocalEntity<Address>(dataContext);
            //this.["Categories"] = new DataViewLocalEntity<Category>(dataContext);
            //this.["Cities"] = new DataViewLocalEntity<City>(dataContext);
            //this.["Countries"] = new DataViewLocalEntity<Country>(dataContext);
            //this.["Customers"] = new DataViewLocalEntity<Customer>(dataContext);
            //this.["Films"] = new DataViewLocalEntity<Film>(dataContext);
            //this.["FilmActors"] = new DataViewLocalEntity<FilmActor>(dataContext);
            //this.["FilmCategories"] = new DataViewLocalEntity<FilmCategory>(dataContext);
            //this.["FilmTexts"] = new DataViewLocalEntity<FilmText>(dataContext);
            //this.["Inventories"] = new DataViewLocalEntity<Inventory>(dataContext);
            //this.["Languages"] = new DataViewLocalEntity<Language>(dataContext);
            //this.["Payments"] = new DataViewLocalEntity<Payment>(dataContext);
            //this.["Rentals"] = new DataViewLocalEntity<Rental>(dataContext);
            //this.["Staffs"] = new DataViewLocalEntity<Staff>(dataContext);
            //this.["Stores"] = new DataViewLocalEntity<Store>(dataContext);
        }

        public DataViewLocalEntity<Actor> Actors { get { return this.GetPropertyValue<Actor>(); } }
        public DataViewLocalEntity<Address> Addresses { get { return this.GetPropertyValue<Address>(); } }
        public DataViewLocalEntity<Category> Categories { get { return this.GetPropertyValue<Category>(); } }
        public DataViewLocalEntity<City> Cities { get { return this.GetPropertyValue<City>(); } }
        public DataViewLocalEntity<Country> Countries { get { return this.GetPropertyValue<Country>(); } }
        public DataViewLocalEntity<Customer> Customers { get { return this.GetPropertyValue<Customer>(); } }
        public DataViewLocalEntity<Film> Films { get { return this.GetPropertyValue<Film>(); } }
        public DataViewLocalEntity<FilmActor> FilmActors { get { return this.GetPropertyValue<FilmActor>(); } }
        public DataViewLocalEntity<FilmCategory> FilmCategories { get { return this.GetPropertyValue<FilmCategory>(); } }
        public DataViewLocalEntity<FilmText> FilmTexts { get { return this.GetPropertyValue<FilmText>(); } }
        public DataViewLocalEntity<Inventory> Inventories { get { return this.GetPropertyValue<Inventory>(); } }
        public DataViewLocalEntity<Language> Languages { get { return this.GetPropertyValue<Language>(); } }
        public DataViewLocalEntity<Payment> Payments { get { return this.GetPropertyValue<Payment>(); } }
        public DataViewLocalEntity<Rental> Rentals { get { return this.GetPropertyValue<Rental>(); } }
        public DataViewLocalEntity<Staff> Staffs { get { return this.GetPropertyValue<Staff>(); } }
        public DataViewLocalEntity<Store> Stores { get { return this.GetPropertyValue<Store>(); } }
    }

    public class RemoteEntityViews : RemoteEntityViewsBase
    {
        public RemoteEntityViews(DataViewDto dataViewDto, DataContext dataContext) : base(dataViewDto, dataContext)
        {
            //this.["Actors"] = new DataViewRemoteEntity<Actor>(dataViewDto, dataContext);
            //this.["Addresses"] = new DataViewRemoteEntity<Address>(dataViewDto, dataContext);
            //this.["Categories"] = new DataViewRemoteEntity<Category>(dataViewDto, dataContext);
            //this.["Cities"] = new DataViewRemoteEntity<City>(dataViewDto, dataContext);
            //this.["Countries"] = new DataViewRemoteEntity<Country>(dataViewDto, dataContext);
            //this.["Customers"] = new DataViewRemoteEntity<Customer>(dataViewDto, dataContext);
            //this.["Films"] = new DataViewRemoteEntity<Film>(dataViewDto, dataContext);
            //this.["FilmActors"] = new DataViewRemoteEntity<FilmActor>(dataViewDto, dataContext);
            //this.["FilmCategories"] = new DataViewRemoteEntity<FilmCategory>(dataViewDto, dataContext);
            //this.["FilmTexts"] = new DataViewRemoteEntity<FilmText>(dataViewDto, dataContext);
            //this.["Inventories"] = new DataViewRemoteEntity<Inventory>(dataViewDto, dataContext);
            //this.["Languages"] = new DataViewRemoteEntity<Language>(dataViewDto, dataContext);
            //this.["Payments"] = new DataViewRemoteEntity<Payment>(dataViewDto, dataContext);
            //this.["Rentals"] = new DataViewRemoteEntity<Rental>(dataViewDto, dataContext);
            //this.["Staffs"] = new DataViewRemoteEntity<Staff>(dataViewDto, dataContext);
            //this.["Stores"] = new DataViewRemoteEntity<Store>(dataViewDto, dataContext);
        }

        public DataViewRemoteEntity<Actor> Actors { get { return this.GetPropertyValue<Actor>(); } }
        public DataViewRemoteEntity<Address> Addresses { get { return this.GetPropertyValue<Address>(); } }
        public DataViewRemoteEntity<Category> Categories { get { return this.GetPropertyValue<Category>(); } }
        public DataViewRemoteEntity<City> Cities { get { return this.GetPropertyValue<City>(); } }
        public DataViewRemoteEntity<Country> Countries { get { return this.GetPropertyValue<Country>(); } }
        public DataViewRemoteEntity<Customer> Customers { get { return this.GetPropertyValue<Customer>(); } }
        public DataViewRemoteEntity<Film> Films { get { return this.GetPropertyValue<Film>(); } }
        public DataViewRemoteEntity<FilmActor> FilmActors { get { return this.GetPropertyValue<FilmActor>(); } }
        public DataViewRemoteEntity<FilmCategory> FilmCategories { get { return this.GetPropertyValue<FilmCategory>(); } }
        public DataViewRemoteEntity<FilmText> FilmTexts { get { return this.GetPropertyValue<FilmText>(); } }
        public DataViewRemoteEntity<Inventory> Inventories { get { return this.GetPropertyValue<Inventory>(); } }
        public DataViewRemoteEntity<Language> Languages { get { return this.GetPropertyValue<Language>(); } }
        public DataViewRemoteEntity<Payment> Payments { get { return this.GetPropertyValue<Payment>(); } }
        public DataViewRemoteEntity<Rental> Rentals { get { return this.GetPropertyValue<Rental>(); } }
        public DataViewRemoteEntity<Staff> Staffs { get { return this.GetPropertyValue<Staff>(); } }
        public DataViewRemoteEntity<Store> Stores { get { return this.GetPropertyValue<Store>(); } }
    }

    public class LocalDtoViews : LocalDtoViewsBase
    {
        public LocalDtoViews(DataContext dataContext, MetadataSrv.Metadata metadataSrv) : base(dataContext, metadataSrv)
        {
            //this.["Actors"] = new DataViewLocalDto<Actor>(dataContext, metadataSrv);
            //this.["Addresses"] = new DataViewLocalDto<Address>(dataContext, metadataSrv);
            //this.["Categories"] = new DataViewLocalDto<Category>(dataContext, metadataSrv);
            //this.["Cities"] = new DataViewLocalDto<City>(dataContext, metadataSrv);
            //this.["Countries"] = new DataViewLocalDto<Country>(dataContext, metadataSrv);
            //this.["Customers"] = new DataViewLocalDto<Customer>(dataContext, metadataSrv);
            //this.["Films"] = new DataViewLocalDto<Film>(dataContext, metadataSrv);
            //this.["FilmActors"] = new DataViewLocalDto<FilmActor>(dataContext, metadataSrv);
            //this.["FilmCategories"] = new DataViewLocalDto<FilmCategory>(dataContext, metadataSrv);
            //this.["FilmTexts"] = new DataViewLocalDto<FilmText>(dataContext, metadataSrv);
            //this.["Inventories"] = new DataViewLocalDto<Inventory>(dataContext, metadataSrv);
            //this.["Languages"] = new DataViewLocalDto<Language>(dataContext, metadataSrv);
            //this.["Payments"] = new DataViewLocalDto<Payment>(dataContext, metadataSrv);
            //this.["Rentals"] = new DataViewLocalDto<Rental>(dataContext, metadataSrv);
            //this.["Staffs"] = new DataViewLocalDto<Staff>(dataContext, metadataSrv);
            //this.["Stores"] = new DataViewLocalDto<Store>(dataContext, metadataSrv);
        }

        public DataViewLocalDto<Actor> Actors { get { return this.GetPropertyValue<Actor>(); } }
        public DataViewLocalDto<Address> Addresses { get { return this.GetPropertyValue<Address>(); } }
        public DataViewLocalDto<Category> Categories { get { return this.GetPropertyValue<Category>(); } }
        public DataViewLocalDto<City> Cities { get { return this.GetPropertyValue<City>(); } }
        public DataViewLocalDto<Country> Countries { get { return this.GetPropertyValue<Country>(); } }
        public DataViewLocalDto<Customer> Customers { get { return this.GetPropertyValue<Customer>(); } }
        public DataViewLocalDto<Film> Films { get { return this.GetPropertyValue<Film>(); } }
        public DataViewLocalDto<FilmActor> FilmActors { get { return this.GetPropertyValue<FilmActor>(); } }
        public DataViewLocalDto<FilmCategory> FilmCategories { get { return this.GetPropertyValue<FilmCategory>(); } }
        public DataViewLocalDto<FilmText> FilmTexts { get { return this.GetPropertyValue<FilmText>(); } }
        public DataViewLocalDto<Inventory> Inventories { get { return this.GetPropertyValue<Inventory>(); } }
        public DataViewLocalDto<Language> Languages { get { return this.GetPropertyValue<Language>(); } }
        public DataViewLocalDto<Payment> Payments { get { return this.GetPropertyValue<Payment>(); } }
        public DataViewLocalDto<Rental> Rentals { get { return this.GetPropertyValue<Rental>(); } }
        public DataViewLocalDto<Staff> Staffs { get { return this.GetPropertyValue<Staff>(); } }
        public DataViewLocalDto<Store> Stores { get { return this.GetPropertyValue<Store>(); } }
    }

    public class RemoteDtoViews : RemoteDtoViewsBase
    {
        public RemoteDtoViews(DataViewDto dataViewDto) : base(dataViewDto)
        {
            //this.["Actors"] = new DataViewRemoteDto<Actor>(dataViewDto);
            //this.["Addresses"] = new DataViewRemoteDto<Address>(dataViewDto);
            //this.["Categories"] = new DataViewRemoteDto<Category>(dataViewDto);
            //this.["Cities"] = new DataViewRemoteDto<City>(dataViewDto);
            //this.["Countries"] = new DataViewRemoteDto<Country>(dataViewDto);
            //this.["Customers"] = new DataViewRemoteDto<Customer>(dataViewDto);
            //this.["Films"] = new DataViewRemoteDto<Film>(dataViewDto);
            //this.["FilmActors"] = new DataViewRemoteDto<FilmActor>(dataViewDto);
            //this.["FilmCategories"] = new DataViewRemoteDto<FilmCategory>(dataViewDto);
            //this.["FilmTexts"] = new DataViewRemoteDto<FilmText>(dataViewDto);
            //this.["Inventories"] = new DataViewRemoteDto<Inventory>(dataViewDto);
            //this.["Languages"] = new DataViewRemoteDto<Language>(dataViewDto);
            //this.["Payments"] = new DataViewRemoteDto<Payment>(dataViewDto);
            //this.["Rentals"] = new DataViewRemoteDto<Rental>(dataViewDto);
            //this.["Staffs"] = new DataViewRemoteDto<Staff>(dataViewDto);
            //this.["Stores"] = new DataViewRemoteDto<Store>(dataViewDto);
        }

        public DataViewRemoteDto Actors { get { return this.GetPropertyValue("Actor"); } }
        public DataViewRemoteDto Addresses { get { return this.GetPropertyValue("Address"); } }
        public DataViewRemoteDto Categories { get { return this.GetPropertyValue("Category"); } }
        public DataViewRemoteDto Cities { get { return this.GetPropertyValue("City"); } }
        public DataViewRemoteDto Countries { get { return this.GetPropertyValue("Country"); } }
        public DataViewRemoteDto Customers { get { return this.GetPropertyValue("Customer"); } }
        public DataViewRemoteDto Films { get { return this.GetPropertyValue("Film"); } }
        public DataViewRemoteDto FilmActors { get { return this.GetPropertyValue("FilmActor"); } }
        public DataViewRemoteDto FilmCategories { get { return this.GetPropertyValue("FilmCategory"); } }
        public DataViewRemoteDto FilmTexts { get { return this.GetPropertyValue("FilmText"); } }
        public DataViewRemoteDto Inventories { get { return this.GetPropertyValue("Inventory"); } }
        public DataViewRemoteDto Languages { get { return this.GetPropertyValue("Language"); } }
        public DataViewRemoteDto Payments { get { return this.GetPropertyValue("Payment"); } }
        public DataViewRemoteDto Rentals { get { return this.GetPropertyValue("Rental"); } }
        public DataViewRemoteDto Staffs { get { return this.GetPropertyValue("Staff"); } }
        public DataViewRemoteDto Stores { get { return this.GetPropertyValue("Store"); } }
    }

    public sealed class Actor : IDerivedEntity
    {
        public Actor(Entity entity)
        {
            if (entity.entityTypeName != "Actor") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public int ActorId { get { return (int)this.entity.dto["ActorId"]; } set { this.entity.dto["ActorId"] = new JValue(value); } }
        public string FirstName { get { return (string)this.entity.dto["FirstName"]; } set { this.entity.dto["FirstName"] = new JValue(value); } }
        public string LastName { get { return (string)this.entity.dto["LastName"]; } set { this.entity.dto["LastName"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int AddressId { get { return (int)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = new JValue(value); } }
        public string Address1 { get { return (string)this.entity.dto["Address1"]; } set { this.entity.dto["Address1"] = new JValue(value); } }
        public string Address2 { get { return (string)(this.entity.dto["Address2"].HasValues ? this.entity.dto["Address2"] : null); } set { this.entity.dto["Address2"] = new JValue(value); } }
        public string District { get { return (string)this.entity.dto["District"]; } set { this.entity.dto["District"] = new JValue(value); } }
        public int CityId { get { return (int)this.entity.dto["CityId"]; } set { this.entity.dto["CityId"] = new JValue(value); } }
        public string PostalCode { get { return (string)(this.entity.dto["PostalCode"].HasValues ? this.entity.dto["PostalCode"] : null); } set { this.entity.dto["PostalCode"] = new JValue(value); } }
        public string Phone { get { return (string)this.entity.dto["Phone"]; } set { this.entity.dto["Phone"] = new JValue(value); } }
        public object Location { get { return (object)this.entity.dto["Location"]; } set { this.entity.dto["Location"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int CategoryId { get { return (int)this.entity.dto["CategoryId"]; } set { this.entity.dto["CategoryId"] = new JValue(value); } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int CityId { get { return (int)this.entity.dto["CityId"]; } set { this.entity.dto["CityId"] = new JValue(value); } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = new JValue(value); } }
        public int CountryId { get { return (int)this.entity.dto["CountryId"]; } set { this.entity.dto["CountryId"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int CountryId { get { return (int)this.entity.dto["CountryId"]; } set { this.entity.dto["CountryId"] = new JValue(value); } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int CustomerId { get { return (int)this.entity.dto["CustomerId"]; } set { this.entity.dto["CustomerId"] = new JValue(value); } }
        public int StoreId { get { return (int)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = new JValue(value); } }
        public string FirstName { get { return (string)this.entity.dto["FirstName"]; } set { this.entity.dto["FirstName"] = new JValue(value); } }
        public string LastName { get { return (string)this.entity.dto["LastName"]; } set { this.entity.dto["LastName"] = new JValue(value); } }
        public string Email { get { return (string)(this.entity.dto["Email"].HasValues ? this.entity.dto["Email"] : null); } set { this.entity.dto["Email"] = new JValue(value); } }
        public int AddressId { get { return (int)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = new JValue(value); } }
        public bool Active { get { return (bool)this.entity.dto["Active"]; } set { this.entity.dto["Active"] = new JValue(value); } }
        public DateTime CreateDate { get { return (DateTime)this.entity.dto["CreateDate"]; } set { this.entity.dto["CreateDate"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int FilmId { get { return (int)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = new JValue(value); } }
        public string Title { get { return (string)this.entity.dto["Title"]; } set { this.entity.dto["Title"] = new JValue(value); } }
        public string Description { get { return (string)(this.entity.dto["Description"].HasValues ? this.entity.dto["Description"] : null); } set { this.entity.dto["Description"] = new JValue(value); } }
        public int? ReleaseYear { get { return (int?)(this.entity.dto["ReleaseYear"].HasValues ? this.entity.dto["ReleaseYear"] : null); } set { this.entity.dto["ReleaseYear"] = new JValue(value); } }
        public int LanguageId { get { return (int)this.entity.dto["LanguageId"]; } set { this.entity.dto["LanguageId"] = new JValue(value); } }
        public int? OriginalLanguageId { get { return (int?)(this.entity.dto["OriginalLanguageId"].HasValues ? this.entity.dto["OriginalLanguageId"] : null); } set { this.entity.dto["OriginalLanguageId"] = new JValue(value); } }
        public int RentalDuration { get { return (int)this.entity.dto["RentalDuration"]; } set { this.entity.dto["RentalDuration"] = new JValue(value); } }
        public float RentalRate { get { return (float)this.entity.dto["RentalRate"]; } set { this.entity.dto["RentalRate"] = new JValue(value); } }
        public int? Length { get { return (int?)(this.entity.dto["Length"].HasValues ? this.entity.dto["Length"] : null); } set { this.entity.dto["Length"] = new JValue(value); } }
        public float ReplacementCost { get { return (float)this.entity.dto["ReplacementCost"]; } set { this.entity.dto["ReplacementCost"] = new JValue(value); } }
        public string Rating { get { return (string)(this.entity.dto["Rating"].HasValues ? this.entity.dto["Rating"] : null); } set { this.entity.dto["Rating"] = new JValue(value); } }
        public string SpecialFeatures { get { return (string)(this.entity.dto["SpecialFeatures"].HasValues ? this.entity.dto["SpecialFeatures"] : null); } set { this.entity.dto["SpecialFeatures"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int ActorId { get { return (int)this.entity.dto["ActorId"]; } set { this.entity.dto["ActorId"] = new JValue(value); } }
        public int FilmId { get { return (int)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int FilmId { get { return (int)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = new JValue(value); } }
        public int CategoryId { get { return (int)this.entity.dto["CategoryId"]; } set { this.entity.dto["CategoryId"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int FilmId { get { return (int)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = new JValue(value); } }
        public string Title { get { return (string)this.entity.dto["Title"]; } set { this.entity.dto["Title"] = new JValue(value); } }
        public string Description { get { return (string)(this.entity.dto["Description"].HasValues ? this.entity.dto["Description"] : null); } set { this.entity.dto["Description"] = new JValue(value); } }
        
        
    }

    public sealed class Inventory : IDerivedEntity
    {
        public Inventory(Entity entity)
        {
            if (entity.entityTypeName != "Inventory") { throw new ArgumentException("Incorrect entity type"); }
            this.entity = entity;
        }

        public Entity entity { get; private set; }
        
        public int InventoryId { get { return (int)this.entity.dto["InventoryId"]; } set { this.entity.dto["InventoryId"] = new JValue(value); } }
        public int FilmId { get { return (int)this.entity.dto["FilmId"]; } set { this.entity.dto["FilmId"] = new JValue(value); } }
        public int StoreId { get { return (int)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int LanguageId { get { return (int)this.entity.dto["LanguageId"]; } set { this.entity.dto["LanguageId"] = new JValue(value); } }
        public string Name { get { return (string)this.entity.dto["Name"]; } set { this.entity.dto["Name"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int PaymentId { get { return (int)this.entity.dto["PaymentId"]; } set { this.entity.dto["PaymentId"] = new JValue(value); } }
        public int CustomerId { get { return (int)this.entity.dto["CustomerId"]; } set { this.entity.dto["CustomerId"] = new JValue(value); } }
        public int StaffId { get { return (int)this.entity.dto["StaffId"]; } set { this.entity.dto["StaffId"] = new JValue(value); } }
        public int? RentalId { get { return (int?)(this.entity.dto["RentalId"].HasValues ? this.entity.dto["RentalId"] : null); } set { this.entity.dto["RentalId"] = new JValue(value); } }
        public float Amount { get { return (float)this.entity.dto["Amount"]; } set { this.entity.dto["Amount"] = new JValue(value); } }
        public DateTime PaymentDate { get { return (DateTime)this.entity.dto["PaymentDate"]; } set { this.entity.dto["PaymentDate"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int RentalId { get { return (int)this.entity.dto["RentalId"]; } set { this.entity.dto["RentalId"] = new JValue(value); } }
        public DateTime RentalDate { get { return (DateTime)this.entity.dto["RentalDate"]; } set { this.entity.dto["RentalDate"] = new JValue(value); } }
        public int InventoryId { get { return (int)this.entity.dto["InventoryId"]; } set { this.entity.dto["InventoryId"] = new JValue(value); } }
        public int CustomerId { get { return (int)this.entity.dto["CustomerId"]; } set { this.entity.dto["CustomerId"] = new JValue(value); } }
        public DateTime? ReturnDate { get { return (DateTime?)(this.entity.dto["ReturnDate"].HasValues ? this.entity.dto["ReturnDate"] : null); } set { this.entity.dto["ReturnDate"] = new JValue(value); } }
        public int StaffId { get { return (int)this.entity.dto["StaffId"]; } set { this.entity.dto["StaffId"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int StaffId { get { return (int)this.entity.dto["StaffId"]; } set { this.entity.dto["StaffId"] = new JValue(value); } }
        public string FirstName { get { return (string)this.entity.dto["FirstName"]; } set { this.entity.dto["FirstName"] = new JValue(value); } }
        public string LastName { get { return (string)this.entity.dto["LastName"]; } set { this.entity.dto["LastName"] = new JValue(value); } }
        public int AddressId { get { return (int)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = new JValue(value); } }
        public string Picture { get { return (string)(this.entity.dto["Picture"].HasValues ? this.entity.dto["Picture"] : null); } set { this.entity.dto["Picture"] = new JValue(value); } }
        public string Email { get { return (string)(this.entity.dto["Email"].HasValues ? this.entity.dto["Email"] : null); } set { this.entity.dto["Email"] = new JValue(value); } }
        public int StoreId { get { return (int)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = new JValue(value); } }
        public bool Active { get { return (bool)this.entity.dto["Active"]; } set { this.entity.dto["Active"] = new JValue(value); } }
        public string Username { get { return (string)this.entity.dto["Username"]; } set { this.entity.dto["Username"] = new JValue(value); } }
        public string Password { get { return (string)(this.entity.dto["Password"].HasValues ? this.entity.dto["Password"] : null); } set { this.entity.dto["Password"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
        
        public int StoreId { get { return (int)this.entity.dto["StoreId"]; } set { this.entity.dto["StoreId"] = new JValue(value); } }
        public int ManagerStaffId { get { return (int)this.entity.dto["ManagerStaffId"]; } set { this.entity.dto["ManagerStaffId"] = new JValue(value); } }
        public int AddressId { get { return (int)this.entity.dto["AddressId"]; } set { this.entity.dto["AddressId"] = new JValue(value); } }
        public DateTime LastUpdate { get { return (DateTime)this.entity.dto["LastUpdate"]; } set { this.entity.dto["LastUpdate"] = new JValue(value); } }
        
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
