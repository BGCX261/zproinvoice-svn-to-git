--------------INVOICE-------------------------------------------------------------------
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_INVOICES_ACCOUNTS_INVOICE_ID')
   BEGIN
        ALTER TABLE [DBO].[Invoices_Accounts] DROP CONSTRAINT FK_INVOICES_ACCOUNTS_INVOICE_ID
   END
GO   
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_INVOICES_SHIPPER_ID')
   BEGIN         
		ALTER TABLE [DBO].[INVOICES] DROP CONSTRAINT FK_INVOICES_SHIPPER_ID
   END
GO   
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_INVOICES_TAXRATE_ID')
   BEGIN    		
        ALTER TABLE [DBO].[INVOICES] DROP CONSTRAINT FK_INVOICES_TAXRATE_ID
   END     
GO    
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_INVOICES_ACCOUNTS_ACCOUNT_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Invoices_Accounts] DROP CONSTRAINT FK_INVOICES_ACCOUNTS_ACCOUNT_ID
   END  
GO          
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_INVOICES_CONTACTS_CONTACT_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Invoices_Contacts] DROP CONSTRAINT FK_INVOICES_CONTACTS_CONTACT_ID
   END  
GO     
---------------------------------------------------------------------------------------------
-----------------------PRODUCTS--------------------------------------------------------------
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCTS_ACCOUNT_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Products] DROP CONSTRAINT FK_PRODUCTS_ACCOUNT_ID
   END  
GO    

IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCTS_PRODUCT_TEMPLATE_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Products] DROP CONSTRAINT FK_PRODUCTS_PRODUCT_TEMPLATE_ID
   END  
GO    
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCTS_CONTACT_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Products] DROP CONSTRAINT FK_PRODUCTS_CONTACT_ID
   END  
GO 
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCTS_CATEGORY_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Products] DROP CONSTRAINT FK_PRODUCTS_CATEGORY_ID
   END  
GO 
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCTS_TYPE_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Products] DROP CONSTRAINT FK_PRODUCTS_TYPE_ID
   END  
GO 
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCTS_MANUFACTURER_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Products] DROP CONSTRAINT FK_PRODUCTS_MANUFACTURER_ID
   END  
GO 
---------------------------------------------------------------------------------------
------------------PRODUCT CATALOG------------------------------------------------------
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCT_TEMPLATES_ACCOUNT_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Product_Templates] DROP CONSTRAINT FK_PRODUCT_TEMPLATES_ACCOUNT_ID
   END  
GO 
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCT_TEMPLATES_CATEGORY_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Product_Templates] DROP CONSTRAINT FK_PRODUCT_TEMPLATES_CATEGORY_ID
   END  
GO 
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCT_TEMPLATES_TYPE_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Product_Templates] DROP CONSTRAINT FK_PRODUCT_TEMPLATES_TYPE_ID
   END  
GO 
IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME LIKE 'FK_PRODUCT_TEMPLATES_MANUFACTURER_ID')
   BEGIN    		
        ALTER TABLE [DBO].[Product_Templates] DROP CONSTRAINT FK_PRODUCT_TEMPLATES_MANUFACTURER_ID
   END  
GO 

---------------------------------------------------------------------------------------------------
----------------Required value for manufacturer----------------------------------------------------
insert into Manufacturers
values (newid(),0,'03AABB61-622A-4FD4-BE35-3A0EB3535E29',getdate(),'03AABB61-622A-4FD4-BE35-3A0EB3535E29',getdate(),'Zinfi','Active',0)
go
insert into Manufacturers
values (newid(),0,'03AABB61-622A-4FD4-BE35-3A0EB3535E29',getdate(),'03AABB61-622A-4FD4-BE35-3A0EB3535E29',getdate(),'Microsoft','Active',0)
go
-----------------------------------------------------------------------------------------------------
update modules
set tab_enabled = 1,
    module_enabled = 1,
    relative_path = '~/CRM/ProductTemplates/index.aspx',
    is_admin = 0
where module_name like '%ProductTemplates%'
go

update modules
set tab_enabled = 1,
    module_enabled = 1,
    relative_path = '~/CRM/ProductTypes/index.aspx',
    is_admin = 0
where module_name like '%ProductTypes%'
go
------------------------------------------------------------
----------------ProductTemplates----------------------------
update shortcuts set relative_path = '~/CRM/ProductTemplates/import.aspx'
where display_name = '.LBL_IMPORT'
go

update shortcuts set relative_path = '~/CRM/ProductTemplates/index.aspx'
where display_name = 'ProductTemplates.LNK_PRODUCT_TEMPLATE_LIST'
go

update shortcuts set relative_path = '~/CRM/Administration/ProductCategories/index.aspx'
where display_name = 'ProductCategories.LNK_PRODUCT_CATEGORIES_LIST'
go

update shortcuts set relative_path = '~/CRM/Administration/ProductTemplates/edit.aspx'
where display_name = 'ProductTemplates.LNK_NEW_PRODUCT_TEMPLATE'
go

update shortcuts set relative_path = '~/CRM/Administration/ProductTypes/index.aspx'
where display_name = 'ProductTypes.LNK_PRODUCT_TYPE_LIST'
go

update shortcuts set relative_path = '~/CRM/Administration/Manufacturers/index.aspx'
where display_name = 'Manufacturers.LNK_MANUFACTURER_LIST'
go
------------------------------------------------------------
-------------Product Type-----------------------------------
update shortcuts set relative_path = '~/CRM/ProductTypes/index.aspx'
where display_name = 'ProductTypes.LNK_PRODUCT_TYPE_LIST'
go

delete from shortcuts
where module_name = 'ProductTypes'
and relative_path = '~/CRM/ProductTemplates/index.aspx'
go

update shortcuts set relative_path = '~/CRM/ProductTypes/edit.aspx'
       ,display_name = 'ProductTypes.LNK_PRODUCT_TYPE_LIST'
where module_name = 'ProductTypes'
and relative_path = '~/CRM/Administration/ProductTemplates/edit.aspx'  
and display_name = 'ProductTemplates.LNK_NEW_PRODUCT_TEMPLATE'  
go

update shortcuts set relative_path = '~/CRM/ProductCategories/index.aspx'
where display_name = 'ProductCategories.LNK_PRODUCT_CATEGORIES_LIST'
go

update shortcuts set relative_path = '~/CRM/Manufacturers/index.aspx'
where display_name = 'Manufacturers.LNK_MANUFACTURER_LIST'
go

update shortcuts set relative_path = '~/CRM/ProductTypes/edit.aspx'
       ,display_name = 'New Product Types'
where image_name = 'CreateProducts.gif'
go

-----------------------------------------------------------------------------------
------------Products---------------------------------------------------------------
update shortcuts set relative_path = '~/CRM/ProductTemplates/edit.aspx'
      ,display_name = 'ProductTemplates.LNK_NEW_PRODUCT_TEMPLATE'
where display_name = 'New Product Types'
and relative_path = '~/CRM/ProductTypes/edit.aspx'
go

update shortcuts set relative_path = '~/CRM/Administration/ProductTypes/index.aspx'
where display_name = 'ProductTypes.LNK_PRODUCT_TYPE_LIST'
go

delete from shortcuts
where module_name = 'ProductTemplates'
and display_name = 'ProductTypes.LNK_PRODUCT_TYPE_LIST'
go

update shortcuts
set display_name = 'New Product Type'
where display_name = 'ProductTemplates.LNK_NEW_PRODUCT_TEMPLATE'
go

update shortcuts
set display_name = 'ProductTemplates.LNK_NEW_PRODUCT_TEMPLATE'
where display_name = 'New Product Type'
and module_name = 'ProductTemplates'
go

update shortcuts
set display_name = 'ProductTemplates.LNK_NEW_PRODUCT'
where display_name = 'New Product Type'
and module_name = 'Products'
go

update shortcuts
set display_name = 'New Product'
where display_name = 'ProductTemplates.LNK_NEW_PRODUCT'
and module_name = 'Products'
go
   
update shortcuts
set display_name = 'New Product Catalog'
where display_name = 'ProductTemplates.LNK_NEW_PRODUCT_TEMPLATE'
and module_name = 'ProductTemplates'
go   
   
update shortcuts
set relative_path = '~/CRM/Products/edit.aspx'
where module_name like 'Products'
and display_name = 'New Product'
go   
---------------------------------------------------------------------------
--------------------Manufacturers------------------------------------------
update modules
set relative_path = '~/CRM/Manufacturers/index.aspx',
is_admin = 0
where module_name like '%manufacturers%' 
---------------------------------------------------------------------------
----------Shortcuts Addition-----------------------------------------------
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','Manufacturers.LNK_MANUFACTURER_LIST','~/CRM/Manufacturers/index.aspx','Manufacturers.gif',1,5,'Manufacturers','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','ProductTemplates.LNK_PRODUCT_TEMPLATE_LIST','~/CRM/ProductTemplates/index.aspx','ProductTemplates.gif',1,6,'ProductTemplates','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','ProductTypes.LNK_PRODUCT_TYPE_LIST','~/CRM/Administration/ProductTypes/index.aspx','ProductTypes.gif',1,7,'Product_Types','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','New Product Type','~/CRM/ProductTemplates/edit.aspx','CreateProducts.gif',1,8,'ProductTemplates','edit')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','ProductCategories.LNK_PRODUCT_CATEGORIES_LIST','~/CRM/ProductCategories/index.aspx','ProductCategories.gif',1,9,'Product_Categories','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','Products.LNK_PRODUCT_LIST','~/CRM/Products/index.aspx','Products.gif',1,10,'Products','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','New Product','~/CRM/Products/edit.aspx','CreateProducts.gif',1,11,'Products','edit')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','.LBL_IMPORT','~/CRM/ProductTemplates/import.aspx','Import.gif',1,12,'ProductTemplates','mport')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','ProductTemplates.LNK_PRODUCT_TEMPLATE_LIST','~/CRM/ProductTemplates/index.aspx','ProductTemplates.gif',1,13,'ProductTemplates','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','Manufacturers.LNK_MANUFACTURER_LIST','~/CRM/Manufacturers/index.aspx','Manufacturers.gif',1,14,'Manufacturers','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','ProductCategories.LNK_PRODUCT_CATEGORIES_LIST','~/CRM/ProductCategories/index.aspx','ProductCategories.gif',1,15,'Product_Categories','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','New Product Catalog','~/CRM/ProductTemplates/edit.aspx','CreateProducts.gif',1,16,'ProductTemplates','edit')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','Manufacturers.LNK_MANUFACTURER_LIST','~/CRM/Manufacturers/index.aspx','Manufacturers.gif',1,17,'Manufacturers','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','New Product Type','~/CRM/ProductTemplates/edit.aspx','CreateProducts.gif',1,18,'ProductTemplates','edit')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','ProductTypes.LNK_PRODUCT_TYPE_LIST','~/CRM/Administration/ProductTypes/index.aspx','ProductTypes.gif',1,19,'Product_Types','list')
go
insert into shortcuts values (newid(),0,NULL,getdate(),'00000000-0000-0000-0000-000000000000',getdate(),'Invoices','ProductCategories.LNK_PRODUCT_CATEGORIES_LIST','~/CRM/ProductCategories/index.aspx','ProductCategories.gif',1,20,'Product_Categories','list')
go
-------------------------------------------------------------------------------------------------------------------------------
-------------Final Adjustment of Invoice Module Links--------------------------------------------------------------------------
delete from shortcuts
where module_name like '%invoices%'
and display_name = 'Manufacturers.LNK_MANUFACTURER_LIST'
and shortcut_order in ('5','14')
go
delete from shortcuts
where module_name like '%invoices%'
and display_name = 'ProductTemplates.LNK_PRODUCT_TEMPLATE_LIST'
and shortcut_order in ('13')
go
delete from shortcuts
where module_name like '%invoices%'
and display_name = 'ProductTypes.LNK_PRODUCT_TYPE_LIST'
and shortcut_order in ('7')
go

update shortcuts
set display_name = 'New Product Catalog'
where shortcut_order in ('8')
and module_name like '%invoices%'
go
update shortcuts
set shortcut_order = 21
where display_name = 'Manufacturers.LNK_MANUFACTURER_LIST'
and module_name like '%invoices%'
go
update shortcuts
set relative_path = '~/CRM/ProductTypes/edit.aspx'
where display_name = 'New Product Type'
and module_name like '%invoices%'
go
delete from shortcuts
where shortcut_order in ('20','12','15')
and module_name like '%invoices%'
go
update shortcuts
set relative_path = '~/CRM/ProductTypes/index.aspx'
where shortcut_order = 16
and module_name like '%invoices%'
go
update shortcuts
set shortcut_order = 11
where shortcut_order = 16
and module_name like '%invoices%'
go
update shortcuts
set shortcut_order = 12
where shortcut_order = 18
and module_name like '%invoices%'
go
update shortcuts
set shortcut_order = 13
where shortcut_order = 21
and module_name like '%invoices%'
go
--------------------------------------------------------------------------------------------------------------------------------
--Alter the stp_Zpro_PRODUCTS_Update
--------------------------------------------------------------------------------------------------------------------------------
ALTER Procedure[dbo].[stp_Zpro_PRODUCTS_Update]  
 ( @ID                    uniqueidentifier output  
 , @MODIFIED_USER_ID      uniqueidentifier  
 , @PRODUCT_TEMPLATE_ID   uniqueidentifier  
 , @NAME                  nvarchar(50)  
 , @STATUS                nvarchar(25)  
 , @ACCOUNT_ID            uniqueidentifier  
 , @CONTACT_ID            uniqueidentifier  
 , @QUANTITY              int  
 , @DATE_PURCHASED        datetime  
 , @DATE_SUPPORT_EXPIRES  datetime  
 , @DATE_SUPPORT_STARTS   datetime  
 , @MANUFACTURER_ID       uniqueidentifier  
 , @CATEGORY_ID           uniqueidentifier  
 , @TYPE_ID               uniqueidentifier  
 , @WEBSITE               nvarchar(255)  
 , @MFT_PART_NUM          nvarchar(50)  
 , @VENDOR_PART_NUM       nvarchar(50)  
 , @SERIAL_NUMBER         nvarchar(50)  
 , @ASSET_NUMBER          nvarchar(50)  
 , @TAX_CLASS             nvarchar(25)  
 , @WEIGHT                float(53)  
 , @CURRENCY_ID           uniqueidentifier  
 , @COST_PRICE            money  
 , @LIST_PRICE            money  
 , @BOOK_VALUE            money  
 , @BOOK_VALUE_DATE       datetime  
 , @DISCOUNT_PRICE        money  
 , @PRICING_FACTOR        int  
 , @PRICING_FORMULA       nvarchar(25)  
 , @SUPPORT_NAME          nvarchar(50)  
 , @SUPPORT_CONTACT       nvarchar(50)  
 , @SUPPORT_DESCRIPTION   nvarchar(255)  
 , @SUPPORT_TERM          nvarchar(25)  
 , @DESCRIPTION           ntext  
 , @TeamID              uniqueidentifier = null  
 )  
as  
  begin  
 set nocount on  
   
 declare @COST_USDOLLAR         money;  
 declare @LIST_USDOLLAR         money;  
 declare @DISCOUNT_USDOLLAR     money;  
  select @COST_USDOLLAR     = @COST_PRICE     / CONVERSION_RATE  
    from CURRENCIES  
   where ID = @CURRENCY_ID;  
  select @LIST_USDOLLAR     = @LIST_PRICE     / CONVERSION_RATE  
    from CURRENCIES  
   where ID = @CURRENCY_ID;  
  select @DISCOUNT_USDOLLAR = @DISCOUNT_PRICE / CONVERSION_RATE  
    from CURRENCIES  
   where ID = @CURRENCY_ID;  
 if not exists(select * from PRODUCTS where ID = @ID) begin -- then  
  if dbo.fnIsEmptyGuid(@ID) = 1 begin -- then  
   set @ID = newid();  
  end -- if;  
  insert into PRODUCTS  
   ( ID                     
   , CREATED_BY             
   , DATE_ENTERED           
   , MODIFIED_USER_ID       
   , DATE_MODIFIED          
   , PRODUCT_TEMPLATE_ID    
   , NAME                   
   , STATUS                 
   , ACCOUNT_ID             
   , CONTACT_ID             
   , QUANTITY               
   , DATE_PURCHASED         
   , DATE_SUPPORT_EXPIRES   
   , DATE_SUPPORT_STARTS    
   , MANUFACTURER_ID        
   , CATEGORY_ID            
   , TYPE_ID                
   , WEBSITE                
   , MFT_PART_NUM           
   , VENDOR_PART_NUM        
   , SERIAL_NUMBER          
   , ASSET_NUMBER           
   , TAX_CLASS              
   , WEIGHT                 
   , CURRENCY_ID            
   , COST_PRICE             
   , COST_USDOLLAR          
   , LIST_PRICE             
   , LIST_USDOLLAR          
   , BOOK_VALUE             
   , BOOK_VALUE_DATE        
   , DISCOUNT_PRICE         
   , DISCOUNT_USDOLLAR      
   , PRICING_FACTOR         
   , PRICING_FORMULA        
   , SUPPORT_NAME           
   , SUPPORT_CONTACT        
   , SUPPORT_DESCRIPTION    
   , SUPPORT_TERM           
   , DESCRIPTION            
   , TEAM_ID 
   , DELETED               
   )  
  values  ( @ID                     
   , @MODIFIED_USER_ID             
   ,  getdate()              
   , @MODIFIED_USER_ID       
   ,  getdate()              
   , @PRODUCT_TEMPLATE_ID    
   , @NAME                   
   , @STATUS                 
   , @ACCOUNT_ID             
   , @CONTACT_ID             
   , @QUANTITY               
   , @DATE_PURCHASED         
   , @DATE_SUPPORT_EXPIRES   
   , @DATE_SUPPORT_STARTS    
   , @MANUFACTURER_ID        
   , @CATEGORY_ID            
   , @TYPE_ID                
   , @WEBSITE                
   , @MFT_PART_NUM           
   , @VENDOR_PART_NUM        
   , @SERIAL_NUMBER          
   , @ASSET_NUMBER           
   , @TAX_CLASS              
   , @WEIGHT                 
   , @CURRENCY_ID      
   , @COST_PRICE             
   , @COST_USDOLLAR          
   , @LIST_PRICE             
   , @LIST_USDOLLAR          
   , @BOOK_VALUE             
   , @BOOK_VALUE_DATE        
   , @DISCOUNT_PRICE         
   , @DISCOUNT_USDOLLAR      
   , @PRICING_FACTOR         
   , @PRICING_FORMULA        
   , @SUPPORT_NAME           
   , @SUPPORT_CONTACT        
   , @SUPPORT_DESCRIPTION    
   , @SUPPORT_TERM           
   , @DESCRIPTION            
   , @TeamID
   , 0
   );  
 end else begin  
  update PRODUCTS  
     set MODIFIED_USER_ID      = @MODIFIED_USER_ID       
       , DATE_MODIFIED         =  getdate()              
       , PRODUCT_TEMPLATE_ID   = @PRODUCT_TEMPLATE_ID    
       , NAME                  = @NAME                   
       , STATUS                = @STATUS                 
       , ACCOUNT_ID            = @ACCOUNT_ID             
       , CONTACT_ID            = @CONTACT_ID             
       , QUANTITY              = @QUANTITY               
       , DATE_PURCHASED        = @DATE_PURCHASED         
       , DATE_SUPPORT_EXPIRES  = @DATE_SUPPORT_EXPIRES   
       , DATE_SUPPORT_STARTS   = @DATE_SUPPORT_STARTS    
       , MANUFACTURER_ID       = @MANUFACTURER_ID        
       , CATEGORY_ID           = @CATEGORY_ID            
       , TYPE_ID               = @TYPE_ID                
       , WEBSITE               = @WEBSITE                
       , MFT_PART_NUM          = @MFT_PART_NUM           
       , VENDOR_PART_NUM       = @VENDOR_PART_NUM        
       , SERIAL_NUMBER         = @SERIAL_NUMBER          
       , ASSET_NUMBER          = @ASSET_NUMBER           
       , TAX_CLASS             = @TAX_CLASS              
       , WEIGHT                = @WEIGHT                 
       , CURRENCY_ID           = @CURRENCY_ID            
       , COST_PRICE            = @COST_PRICE             
       , COST_USDOLLAR         = @COST_USDOLLAR          
       , LIST_PRICE            = @LIST_PRICE             
       , LIST_USDOLLAR         = @LIST_USDOLLAR          
       , BOOK_VALUE            = @BOOK_VALUE             
       , BOOK_VALUE_DATE       = @BOOK_VALUE_DATE        
       , DISCOUNT_PRICE        = @DISCOUNT_PRICE         
       , DISCOUNT_USDOLLAR     = @DISCOUNT_USDOLLAR      
       , PRICING_FACTOR        = @PRICING_FACTOR         
       , PRICING_FORMULA       = @PRICING_FORMULA        
       , SUPPORT_NAME          = @SUPPORT_NAME           
       , SUPPORT_CONTACT       = @SUPPORT_CONTACT        
       , SUPPORT_DESCRIPTION   = @SUPPORT_DESCRIPTION    
       , SUPPORT_TERM          = @SUPPORT_TERM           
       , DESCRIPTION           = @DESCRIPTION            
       , TEAM_ID               = @TeamID
   where ID                    = @ID                   ;  
 end -- if;  
end  
  
  





