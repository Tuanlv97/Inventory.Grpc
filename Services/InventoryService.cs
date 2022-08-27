namespace Inventory.Grpc.Services;

using Inventory.Grpc.Protos;
using Inventory.Grpc.Repositories;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

public class InventoryService : StockProtoService.StockProtoServiceBase
{
    private readonly InventoryRepository _inventoryRepository;
    private readonly ILogger _logger;
    public InventoryService(InventoryRepository inventoryRepository, ILogger logger)
    {
        _inventoryRepository = inventoryRepository ?? throw new ArgumentNullException(nameof(inventoryRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<StockModel> GetStock(GetStockRequest request, global::Grpc.Core.ServerCallContext context)
    {
        _logger.Information($"BEGIN Get Stock of ItemNo : {request.ItemNo}");
        var stockQuantity = await _inventoryRepository.GetStockQuantity(request.ItemNo);
        var result = new StockModel()
        {
            Quantity = stockQuantity
        };

        _logger.Information($"END Get Stock of ItemNo : {request.ItemNo} quantity: {result.Quantity}");
        return result;
    }
}
