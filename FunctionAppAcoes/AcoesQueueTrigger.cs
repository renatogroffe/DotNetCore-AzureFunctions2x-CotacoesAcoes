using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using FunctionAppAcoes.Models;
using FunctionAppAcoes.Entities;

namespace FunctionAppAcoes
{
    public static class AcoesQueueTrigger
    {
        [FunctionName("AcoesQueueTrigger")]
        public static void Run([QueueTrigger("queue-acoes", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            var cotacao =
                JsonConvert.DeserializeObject<Acao>(myQueueItem);

            if (!String.IsNullOrWhiteSpace(cotacao.Codigo) &&
                cotacao.Valor.HasValue && cotacao.Valor > 0)
            {
                cotacao.Codigo = cotacao.Codigo.Trim().ToUpper();

                var storageAccount = CloudStorageAccount
                    .Parse(Environment.GetEnvironmentVariable("AzureWebJobsStorage"));
                var acaoTable = storageAccount
                    .CreateCloudTableClient().GetTableReference("CotacaoAcoes");
                if (acaoTable.CreateIfNotExistsAsync().Result)
                    log.LogInformation("Criando a tabela CotacaoAcoes...");

                AcaoEntity dadosAcao =
                    new AcaoEntity(
                        cotacao.Codigo,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dadosAcao.Valor = cotacao.Valor.Value;
                var insertOperation = TableOperation.Insert(dadosAcao);
                var resultInsert = acaoTable.ExecuteAsync(insertOperation).Result;

                log.LogInformation($"MoedasQueueTrigger: {myQueueItem}");
            }
            else
                log.LogError($"MoedasQueueTrigger - Erro validação: {myQueueItem}");
        }
    }
}