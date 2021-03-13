using Microsoft.EntityFrameworkCore;
using Persistance;
using Persistance.Entities;
using System.Collections.Generic;

namespace mySimpleMessageService.Persistance
{
    public class Seed
    {
        public static void SeedData(MessageServiceContext context)
        {
            var cleanupMessages = @"
                DELETE FROM dbo.Messages
                ";

            context.Database.ExecuteSqlRaw(cleanupMessages);

            var cleanupContacts = @"
                DELETE FROM dbo.Contacts
                ";

            context.Database.ExecuteSqlRaw(cleanupContacts);


            var cleanupEvents = @"
                DELETE FROM dbo.Events
                ";

            context.Database.ExecuteSqlRaw(cleanupEvents);


            var commandText =
                @"INSERT INTO dbo.Contacts ([Name], [Surname])
                VALUES 
                 ('Mateusz', 'Przybylek')
                ,('Adam', 'Malysz')
                ";
            context.Database.ExecuteSqlRaw(commandText);

            //commandText =
            //   @"INSERT INTO dbo.Messages ([Text], [MessageType], [ContactSentId], [ContactReceivedId])
            //    VALUES 
            //     ('Message1','0',1,2)
            //    ,('Message2','0',1,2)
            //    ,('Message3','0',1,2)
            //    ,('Message4','0',2,1)
            //    ,('Message5','0',2,1)
            //    ,('Message6','0',2,1)
            //    ";

            //context.Database.ExecuteSqlRaw(commandText);
        }
    }
}
