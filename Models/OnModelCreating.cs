protected override void OnModelCreating(ModelBuilder modelBuilder)
{
	modelBuilder.Entity<Categories>(entity =>
	{
		entity.Property(e => e.CategoryID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.CategoryName)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.Description)
			.HasColumnType("ntext")
;
		entity.Property(e => e.Picture)
			.HasColumnType("image")
;
	});
	modelBuilder.Entity<CustomerCustomerDemo>(entity =>
	{
		entity.Property(e => e.CustomerID)
			.HasColumnType("nchar")
			.IsRequired()
;
		entity.Property(e => e.CustomerTypeID)
			.HasColumnType("nchar")
			.IsRequired()
;
	});
	modelBuilder.Entity<CustomerDemographics>(entity =>
	{
		entity.Property(e => e.CustomerTypeID)
			.HasColumnType("nchar")
			.IsRequired()
;
		entity.Property(e => e.CustomerDesc)
			.HasColumnType("ntext")
;
	});
	modelBuilder.Entity<Customers>(entity =>
	{
		entity.Property(e => e.CustomerID)
			.HasColumnType("nchar")
			.IsRequired()
;
		entity.Property(e => e.CompanyName)
			.HasMaxLength(40)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.ContactName)
			.HasMaxLength(30)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.ContactTitle)
			.HasMaxLength(30)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Address)
			.HasMaxLength(60)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.City)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Region)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.PostalCode)
			.HasMaxLength(10)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Country)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Phone)
			.HasMaxLength(24)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Fax)
			.HasMaxLength(24)
			.HasColumnType("nvarchar")
;
	});
	modelBuilder.Entity<Employees>(entity =>
	{
		entity.Property(e => e.EmployeeID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.LastName)
			.HasMaxLength(20)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.FirstName)
			.HasMaxLength(10)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.Title)
			.HasMaxLength(30)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.TitleOfCourtesy)
			.HasMaxLength(25)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.BirthDate)
			.HasColumnType("datetime")
;
		entity.Property(e => e.HireDate)
			.HasColumnType("datetime")
;
		entity.Property(e => e.Address)
			.HasMaxLength(60)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.City)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Region)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.PostalCode)
			.HasMaxLength(10)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Country)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.HomePhone)
			.HasMaxLength(24)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Extension)
			.HasMaxLength(4)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Photo)
			.HasColumnType("image")
;
		entity.Property(e => e.Notes)
			.HasColumnType("ntext")
;
		entity.Property(e => e.ReportsTo)
			.HasColumnType("int")
;
		entity.Property(e => e.PhotoPath)
			.HasMaxLength(255)
			.HasColumnType("nvarchar")
;
	});
	modelBuilder.Entity<EmployeeTerritories>(entity =>
	{
		entity.Property(e => e.EmployeeID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.TerritoryID)
			.HasMaxLength(20)
			.HasColumnType("nvarchar")
			.IsRequired()
;
	});
	modelBuilder.Entity<Order Details>(entity =>
	{
		entity.Property(e => e.OrderID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.ProductID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.UnitPrice)
			.HasColumnType("money")
			.IsRequired()
;
		entity.Property(e => e.Quantity)
			.HasColumnType("smallint")
			.IsRequired()
;
		entity.Property(e => e.Discount)
			.HasColumnType("real")
			.IsRequired()
;
	});
	modelBuilder.Entity<Orders>(entity =>
	{
		entity.Property(e => e.OrderID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.CustomerID)
			.HasColumnType("nchar")
;
		entity.Property(e => e.EmployeeID)
			.HasColumnType("int")
;
		entity.Property(e => e.OrderDate)
			.HasColumnType("datetime")
;
		entity.Property(e => e.RequiredDate)
			.HasColumnType("datetime")
;
		entity.Property(e => e.ShippedDate)
			.HasColumnType("datetime")
;
		entity.Property(e => e.ShipVia)
			.HasColumnType("int")
;
		entity.Property(e => e.Freight)
			.HasColumnType("money")
;
		entity.Property(e => e.ShipName)
			.HasMaxLength(40)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.ShipAddress)
			.HasMaxLength(60)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.ShipCity)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.ShipRegion)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.ShipPostalCode)
			.HasMaxLength(10)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.ShipCountry)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
	});
	modelBuilder.Entity<Products>(entity =>
	{
		entity.Property(e => e.ProductID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.ProductName)
			.HasMaxLength(40)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.SupplierID)
			.HasColumnType("int")
;
		entity.Property(e => e.CategoryID)
			.HasColumnType("int")
;
		entity.Property(e => e.QuantityPerUnit)
			.HasMaxLength(20)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.UnitPrice)
			.HasColumnType("money")
;
		entity.Property(e => e.UnitsInStock)
			.HasColumnType("smallint")
;
		entity.Property(e => e.UnitsOnOrder)
			.HasColumnType("smallint")
;
		entity.Property(e => e.ReorderLevel)
			.HasColumnType("smallint")
;
		entity.Property(e => e.Discontinued)
			.HasColumnType("bit")
			.IsRequired()
;
	});
	modelBuilder.Entity<Region>(entity =>
	{
		entity.Property(e => e.RegionID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.RegionDescription)
			.HasColumnType("nchar")
			.IsRequired()
;
	});
	modelBuilder.Entity<Shippers>(entity =>
	{
		entity.Property(e => e.ShipperID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.CompanyName)
			.HasMaxLength(40)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.Phone)
			.HasMaxLength(24)
			.HasColumnType("nvarchar")
;
	});
	modelBuilder.Entity<Suppliers>(entity =>
	{
		entity.Property(e => e.SupplierID)
			.HasColumnType("int")
			.IsRequired()
;
		entity.Property(e => e.CompanyName)
			.HasMaxLength(40)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.ContactName)
			.HasMaxLength(30)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.ContactTitle)
			.HasMaxLength(30)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Address)
			.HasMaxLength(60)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.City)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Region)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.PostalCode)
			.HasMaxLength(10)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Country)
			.HasMaxLength(15)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Phone)
			.HasMaxLength(24)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.Fax)
			.HasMaxLength(24)
			.HasColumnType("nvarchar")
;
		entity.Property(e => e.HomePage)
			.HasColumnType("ntext")
;
	});
	modelBuilder.Entity<Territories>(entity =>
	{
		entity.Property(e => e.TerritoryID)
			.HasMaxLength(20)
			.HasColumnType("nvarchar")
			.IsRequired()
;
		entity.Property(e => e.TerritoryDescription)
			.HasColumnType("nchar")
			.IsRequired()
;
		entity.Property(e => e.RegionID)
			.HasColumnType("int")
			.IsRequired()
;
	});
}
