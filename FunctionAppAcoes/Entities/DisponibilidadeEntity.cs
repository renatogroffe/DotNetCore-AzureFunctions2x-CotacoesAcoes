using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppAcoes.Entities
{
    public class DisponibilidadeEntity : TableEntity
    {
        public DisponibilidadeEntity(string local, string horario)
        {
            PartitionKey = local;
            RowKey = horario;
        }

        public DisponibilidadeEntity() { }
        
        public string Mensagem { get; set; }
    }
}