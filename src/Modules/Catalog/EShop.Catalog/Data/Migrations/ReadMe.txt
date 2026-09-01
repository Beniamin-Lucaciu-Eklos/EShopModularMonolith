
#adding migration
Add-Migration InitialCreate
            -OutputDir Data/Migrations
            -Project Modules\Catalog\EShop.Catalog
            -StartupProject Bootstrapper\EShop.Api

