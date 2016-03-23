using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using SKYPE4COMLib;

namespace SkypeHelper
{
    class Program
    {
        static readonly List<string> Names = new List<string> { "contact1", "contact2", "contact3" };
        static readonly List<string> ChatNames = new List<string> { "#paste chat unique name"};
        static SkypeConfiguration _config = new SkypeConfiguration(10);
        static void Main(string[] args)
        {
            var skype = new Skype();
            while (true)
            {
                if (!skype.Client.IsRunning)
                    skype.Client.Start(true, true);

                var unreaded = GetUnreadMessages(skype);

                SendUnreadedToMail(unreaded);
                Thread.Sleep(10000);
            }
        }

        private static IEnumerable<ChatMessage> GetUnreadMessages(ISkype skype)
        {
            ChatMessageCollection messages = null;
            foreach (var name in Names)
                messages = skype.Messages[name];
            foreach (string chatName in ChatNames)
            {
                var chatMessages = skype.Chat[chatName].Messages;
                if (messages == null) break;
                    foreach (ChatMessage chatMessage in chatMessages)
                {
                     messages.Add(chatMessage);
                }
            }

            var unreaded = messages?.OfType<ChatMessage>().Where(o => (int)o.Status == 2);
            return unreaded;

            //            foreach (Chat chat in skype.Chats)
            //            {
            //                if (chat.ActiveMembers.Count > 2)
            //                {
            //                    Console.WriteLine(chat.Name);
            //                    foreach (User activeMember in chat.ActiveMembers)
            //                    {
            //                        Console.WriteLine(activeMember.FullName);
            //                    }
            //                    Console.WriteLine(chat.ActiveMembers.Count);
            //                }
            //            }
        }

        private static void SendUnreadedToMail(IEnumerable<ChatMessage> unreaded)
        {
            var chatMessages = unreaded as ChatMessage[] ?? unreaded.ToArray();
            if(unreaded!= null && !chatMessages.Any()) return;
            
            foreach (var message in chatMessages)
            {
                var body = string.Empty;
                var client = SmtpClientHelper.SetCredentails(_config);
                body = body + $"{message.FromDisplayName}: {message.Body}\n";

                var mail = new MailMessage(SmtpClientHelper.GetMailAdress(_config), "996556888888@sms.megacom.kg")
                {
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                    Subject = "Skype message: " + DateTime.Now,
                };

                mail.Body = body + " " + DateTime.Now;

                try
                {
                    client.Send(mail);
                    message.Seen = true;
                    Console.Write(mail.Body);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
                Thread.Sleep(10000);
                _config.CurrentCount ++;
            }
        }

        private static void PrintUnreadedMessages(IEnumerable<ChatMessage> unreaded)
        {
            foreach (var chatMessage in unreaded)
            {
                Console.WriteLine(chatMessage.Body);
            }
        }
    }
}
