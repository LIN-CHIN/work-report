using RabbitMQ.Client;

namespace PublishWorker.RabbitMQ
{
    /// <summary>
    /// 實現RabbitMQ功能的Helper Interface
    /// </summary>
    public interface IRabbitMQHelper
    {
        /// <summary>
        /// 建立RabbitMQ連線
        /// </summary>
        /// <returns></returns>
        IConnection Connect();

        /// <summary>
        /// 建立RabbitMQ Model
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        IModel CreateModel(IConnection connection);
    }
}
