using System;
using System.Collections.Generic;
using System.Text;

namespace MiniCommerce.Shared.Events;

public record OrderCreatedEvent(
    Guid OrderId,
    Guid UserId,
    Guid ProductId,
    int Quantity,
    decimal TotalPrice
);
