namespace ChatService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatServiceContext : DbMigration
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
                        User_UserGuid = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ConnectionId)
                .ForeignKey("dbo.Users", t => t.User_UserGuid)
                .Index(t => t.User_UserGuid);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupGuid = c.String(nullable: false, maxLength: 128),
                        GroupId = c.Int(nullable: false),
                        GroupName = c.String(),
                    })
                .PrimaryKey(t => t.GroupGuid);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserGuid = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserGuid);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        User_UserGuid = c.String(nullable: false, maxLength: 128),
                        Group_GroupGuid = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.User_UserGuid, t.Group_GroupGuid })
                .ForeignKey("dbo.Users", t => t.User_UserGuid, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_GroupGuid, cascadeDelete: true)
                .Index(t => t.User_UserGuid)
                .Index(t => t.Group_GroupGuid);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserGroups", "Group_GroupGuid", "dbo.Groups");
            DropForeignKey("dbo.UserGroups", "User_UserGuid", "dbo.Users");
            DropForeignKey("dbo.Connections", "User_UserGuid", "dbo.Users");
            DropIndex("dbo.UserGroups", new[] { "Group_GroupGuid" });
            DropIndex("dbo.UserGroups", new[] { "User_UserGuid" });
            DropIndex("dbo.Connections", new[] { "User_UserGuid" });
            DropTable("dbo.UserGroups");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
            DropTable("dbo.Connections");
            DropTable("dbo.ChatMessages");
        }
    }
}
