namespace ChatService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        MessageState = c.Int(nullable: false),
                        SendTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateTable(
                "dbo.Connections",
                c => new
                    {
                        ConnectionId = c.String(nullable: false, maxLength: 128),
                        UserAgent = c.String(),
                        Connected = c.Boolean(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ConnectionId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        GroupGuid = c.String(),
                        GroupName = c.String(),
                    })
                .PrimaryKey(t => t.GroupId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_UserId = c.Int(nullable: false),
                        Group_GroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.Group_GroupId })
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_GroupId, cascadeDelete: true)
                .Index(t => t.User_UserId)
                .Index(t => t.Group_GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "Group_GroupId", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Connections", "User_UserId", "dbo.Users");
            DropIndex("dbo.UserGroups", new[] { "Group_GroupId" });
            DropIndex("dbo.UserGroups", new[] { "User_UserId" });
            DropIndex("dbo.Connections", new[] { "User_UserId" });
            DropTable("dbo.UserGroups");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
            DropTable("dbo.Connections");
            DropTable("dbo.ChatMessages");
        }
    }
}
