CREATE PROCEDURE AddProductToWarehouse
    @IdProduct INT,
    @IdWarehouse INT,
    @Amount INT,
    @CreatedAt DATETIME
AS
BEGIN
    DECLARE @IdProductFromDb INT;
    DECLARE @IdOrder INT;
    DECLARE @Price DECIMAL(10,2);

    -- Find matching order that has not yet been fulfilled
    SELECT TOP 1 @IdOrder = o.IdOrder
    FROM [Order] o
             LEFT JOIN Product_Warehouse pw ON o.IdOrder = pw.IdOrder
    WHERE o.IdProduct = @IdProduct
      AND o.Amount = @Amount
      AND pw.IdProductWarehouse IS NULL
      AND o.CreatedAt < @CreatedAt;

    -- Get product info
    SELECT
        @IdProductFromDb = IdProduct,
        @Price = Price
    FROM Product
    WHERE IdProduct = @IdProduct;

    -- Validate product exists
    IF @IdProductFromDb IS NULL
        BEGIN
            RAISERROR('Invalid parameter: Provided IdProduct does not exist', 16, 1);
            RETURN;
        END;

    -- Validate order found
    IF @IdOrder IS NULL
        BEGIN
            RAISERROR('Invalid parameter: There is no order to fulfill', 16, 1);
            RETURN;
        END;

    -- Validate warehouse exists
    IF NOT EXISTS(SELECT 1 FROM Warehouse WHERE IdWarehouse = @IdWarehouse)
        BEGIN
            RAISERROR('Invalid parameter: Provided IdWarehouse does not exist', 16, 1);
            RETURN;
        END;

    -- Begin atomic operation
    SET XACT_ABORT ON;
    BEGIN TRAN;

    -- Fulfill the order
    UPDATE [Order]
    SET FulfilledAt = @CreatedAt
    WHERE IdOrder = @IdOrder;

    -- Insert the fulfilled record
    INSERT INTO Product_Warehouse(
        IdWarehouse,
        IdProduct,
        IdOrder,
        Amount,
        Price,
        CreatedAt
    )
    VALUES (
               @IdWarehouse,
               @IdProduct,
               @IdOrder,
               @Amount,
               @Amount * @Price,
               @CreatedAt
           );

    -- Return new ID
    SELECT SCOPE_IDENTITY() AS NewId;

    COMMIT;
END;
