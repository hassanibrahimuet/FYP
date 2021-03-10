namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPhonePropertyToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "phone");
        }
    }
}
