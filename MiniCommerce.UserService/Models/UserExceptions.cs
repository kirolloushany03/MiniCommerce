namespace MiniCommerce.UserService.Models;

public class UserNotFoundException()
    : Exception("user not found");

public class InvalidAmountException()
    : Exception($"The amount is invalid. It must be greater than zero.");

public class EmailAlreadyInUseException()
    : Exception($"The email is already registerd.");

public class InsufficientFundsException()
    : Exception("Insufficient funds to complete the transaction.");
