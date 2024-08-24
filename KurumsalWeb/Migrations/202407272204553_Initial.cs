namespace KurumsalWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Iletisim", "Email", c => c.String());
            DropColumn("dbo.Iletisim", "Fax");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Iletisim", "Fax", c => c.String());
            DropColumn("dbo.Iletisim", "Email");
        }
    }
}
