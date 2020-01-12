using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionAppAcoes.Entities
{
    public class AcaoEntity : TableEntity
    {
        public AcaoEntity(string codigo, string horario)
        {
            PartitionKey = codigo;
            RowKey = horario;
        }

        public AcaoEntity() { }
        
        public double Valor { get; set; }
        
    }
}