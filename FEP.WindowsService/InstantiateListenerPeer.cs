using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Switch.Core;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace FEP.WindowsService
{
    public class InstantiateListenerPeer
    {
        public InstantiateListenerPeer()
        {
            SourceNode sourceNode = new SourceNode
            {
                Name = ConfigurationSettings.AppSettings["NodeNameThroughSwitch"],
                IPAddress = ConfigurationSettings.AppSettings["NodeIPThroughSwitch"],
                Port = ConfigurationSettings.AppSettings["NodePortThroughSwitch"]
                
            };
            TcpListener tcpListener = new TcpListener(int.Parse(sourceNode.Port));
            tcpListener.LocalInterface = sourceNode.IPAddress;
            ListenerPeer listener = new ListenerPeer(sourceNode.Id.ToString(), new TwoBytesNboHeaderChannel
                    (new Iso8583Ascii1987BinaryBitmapMessageFormatter(), sourceNode.IPAddress, int.Parse(sourceNode.Port)),
                     new BasicMessagesIdentifier(11, 41), tcpListener);
            listener.Receive += new PeerReceiveEventHandler(Listener_Receive);
            Console.WriteLine("Listening on Source {0} on port {1}", sourceNode.Name, sourceNode.Port);
            tcpListener.Start();
        }

        static void Listener_Receive(object sender, ReceiveEventArgs e)
        {
            //Cast event sender as ClientPeer
            ListenerPeer sourcePeer = sender as ListenerPeer;

            // Get source node from client - client Name = SourceNode ID
            long sourceID = Convert.ToInt64(sourcePeer.Name); //wher message is coming from

            //then use the ID to retrieve the source node

            //Get the Message received
            Iso8583Message originalMessage = e.Message as Iso8583Message;

            //continue coding
            try
            {
                SourceNode sourceNode = new SourceNode
                {
                    Name = ConfigurationSettings.AppSettings["NodeNameTroughSwitch"],
                    IPAddress = ConfigurationSettings.AppSettings["NodeIPThroughSwitch"],
                    Port = ConfigurationSettings.AppSettings["NodePortThroughSwitch"]

                };
                CheckSourceNode checkSourceNode = new CheckSourceNode();
                //checkSourceNode.SourceNode(sourceNode, originalMessage);
                checkSourceNode.SourceNode(originalMessage);
                //originalMessage.SetResponseMessageTypeIdentifier();
                sourcePeer.Send(originalMessage);
                sourcePeer.Listener.Start();
            }
            catch (Exception ex)
            {
                LogErrors.WriteErrorLog(ex);
            }
            
        }
    }
}
