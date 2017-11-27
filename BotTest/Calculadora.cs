using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using System.Diagnostics;
using Take.Blip.Client;
using Lime.Messaging.Contents;
using System.Linq;
using System.Collections.Generic;
using Take.Blip.Client.Extensions.Broadcast;
using Take.Blip.Client.Extensions.Bucket;

namespace BotTest
{
    public class Calculadora : IMessageReceiver
    {
        private readonly ISender _sender;

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {   
            if(Program.valores.Count == 0)
            {
                try
                {
                    Program.valores.Add(double.Parse(message.Content.ToString()));
                    await _sender.SendMessageAsync("Digite o segundo valor", message.From, cancellationToken);
                }
                catch (FormatException e)
                {
                    await _sender.SendMessageAsync("Olá, seja bem-vindo! \n Você está na Calculadora do Dioni! \n Digite o primeiro valor: ", message.From, cancellationToken);
                }
                          
            }
            else if(Program.valores.Count == 1)
            {
                Program.valores.Add(double.Parse(message.Content.ToString()));
                var document = new Select
                {
                    Text = "Escolha uma operação:",
                    Options = new[]
                {
                    new SelectOption
                    {
                        Order = 1,
                        Text = "Soma",
                        Value = new PlainText { Text = "1" }
                    },
                    new SelectOption
                    {
                        Order = 2,
                        Text = "Subtração",
                        Value = new PlainText { Text = "2" }
                    },
                    new SelectOption
                    {
                        Order = 3,
                        Text = "Divisão",
                        Value = new PlainText { Text = "3" }
                    },
                    new SelectOption
                    {
                        Order = 4,
                        Text = "Multiplição",
                        Value = new PlainText { Text = "4" }
                    }
                }
                };

                await _sender.SendMessageAsync(document, message.From, cancellationToken);

            }
            else if (Program.valores.Count == 2)
            {
                if (message.Content.ToString().Equals("3") && Program.valores[1] == 0)
                {
                    Program.valores.RemoveAt(1);
                   await _sender.SendMessageAsync("Ops... Você está tentando dividir por zero! \n Digite valor maior que zero!", message.From, cancellationToken);
                }
                else
                {
                    double resultado = realizaOperacao(message.Content.ToString());
                    Program.valores = new List<double>();
                    await _sender.SendMessageAsync("O total: " + resultado +"\n Faça uma nova operação, \n digite o primeiro valor: ", message.From, cancellationToken);
                }
                
            }

        }
        public Calculadora(ISender sender)
        {
            _sender = sender;
        }

        public double realizaOperacao(string operacao)
        {
            List<double> valores = Program.valores;
            switch (operacao)
            {
                case "1":
                    return valores[0] + valores[1];
                case "2":
                    return valores[0] - valores[1];
                case "3":
                    return valores[0] / valores[1];
                case "4":
                    return valores[0] * valores[1];
                default:
                    return 0;
            }
            
            
        }
        
    }
}
