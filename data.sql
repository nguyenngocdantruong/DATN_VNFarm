-- Tạo cơ sở dữ liệu VNFarm
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VNFarm')
BEGIN
    CREATE DATABASE VNFarm;
END
GO

USE VNFarm;
GO

-- Tạo bảng Users
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        PasswordHash NVARCHAR(255) NOT NULL,
        PhoneNumber NVARCHAR(20) NULL,
        Address NVARCHAR(255) NULL,
        ImageUrl NVARCHAR(255) NULL,
        Role INT NOT NULL DEFAULT 3, -- UserRole: Admin = 1, Seller = 2, Buyer = 3, User = -1, All = -999
        IsActive BIT NOT NULL DEFAULT 1,
        EmailVerified BIT NOT NULL DEFAULT 0,
        EmailNotificationsEnabled BIT NOT NULL DEFAULT 1,
        OrderStatusNotificationsEnabled BIT NOT NULL DEFAULT 1,
        DiscountNotificationsEnabled BIT NOT NULL DEFAULT 1,
        AdminNotificationsEnabled BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0
    );

    -- Tạo chỉ mục cho email để tìm kiếm nhanh và đảm bảo duy nhất
    CREATE UNIQUE INDEX IX_Users_Email ON Users(Email) WHERE IsDeleted = 0;
END
GO

-- Tạo bảng Stores
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Stores')
BEGIN
    CREATE TABLE Stores (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        LogoUrl NVARCHAR(255) NULL,
        Address NVARCHAR(255) NULL,
        PhoneNumber NVARCHAR(20) NULL,
        Email NVARCHAR(100) NULL,
        BusinessType INT NOT NULL DEFAULT 1, -- StoreType: Farmer = 1, Retailer = 2, Wholesaler = 3, All = -999
        IsActive BIT NOT NULL DEFAULT 1,
        VerificationStatus INT NOT NULL DEFAULT 0, -- StoreStatus: Pending = 0, Verified = 1, Rejected = 2, Suspended = 3
        AverageRating DECIMAL(3,2) NOT NULL DEFAULT 5.0,
        ReviewCount INT NOT NULL DEFAULT 0,
        UserId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Stores_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng Categories
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        MinPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        MaxPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        Description NVARCHAR(500) NULL,
        IconUrl NVARCHAR(255) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0
    );
END
GO

-- Tạo bảng Products
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        Price DECIMAL(18,2) NOT NULL,
        ImageUrl NVARCHAR(255) NULL,
        StockQuantity INT NOT NULL DEFAULT 0,
        Unit INT NOT NULL DEFAULT 1, -- Unit: Kg = 1, Box = 2, Piece = 3, etc.
        SoldQuantity DECIMAL(18,2) NOT NULL DEFAULT 0,
        StoreId INT NOT NULL,
        CategoryId INT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        Origin NVARCHAR(100) NULL,
        AverageRating DECIMAL(3,2) NOT NULL DEFAULT 0,
        TotalSoldQuantity INT NOT NULL DEFAULT 0,
        ReviewCount INT NOT NULL DEFAULT 0,
        ReviewStar1Count INT NOT NULL DEFAULT 0,
        ReviewStar2Count INT NOT NULL DEFAULT 0,
        ReviewStar3Count INT NOT NULL DEFAULT 0,
        ReviewStar4Count INT NOT NULL DEFAULT 0,
        ReviewStar5Count INT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Products_Stores FOREIGN KEY (StoreId) REFERENCES Stores(Id),
        CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
    );
END
GO

-- Tạo bảng Discounts
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Discounts')
BEGIN
    CREATE TABLE Discounts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(50) NOT NULL,
        Description NVARCHAR(255) NULL,
        RemainingQuantity INT NOT NULL DEFAULT 0,
        Status INT NOT NULL DEFAULT 0, -- DiscountStatus: Active = 0, Inactive = 1, All = -999
        StartDate DATETIME NOT NULL,
        EndDate DATETIME NOT NULL,
        Type INT NOT NULL DEFAULT 0, -- DiscountType: Percentage = 0, FixedAmount = 1, FreeShipping = 2, All = -999
        DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        MinimumOrderAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        MaximumDiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        StoreId INT NULL,
        UserId INT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Discounts_Stores FOREIGN KEY (StoreId) REFERENCES Stores(Id),
        CONSTRAINT FK_Discounts_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng Orders
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderCode NVARCHAR(50) NOT NULL,
        Status INT NOT NULL DEFAULT 0, -- OrderStatus: Pending = 0, Processing = 1, Shipped = 2, Delivered = 3, Cancelled = 4, Refunded = 5
        Notes NVARCHAR(500) NULL,
        TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        ShippingFee DECIMAL(18,2) NOT NULL DEFAULT 0,
        TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        FinalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        OrderPaymentId BIGINT NOT NULL DEFAULT 0,
        PaymentStatus INT NOT NULL DEFAULT 1, -- PaymentStatus: Unpaid = 1, PartiallyPaid = 2, Paid = 3, Refunded = 4, Failed = 5, WaitRefund = 6
        PaymentMethod INT NOT NULL DEFAULT 2, -- PaymentMethodEnum: PaymentAfter = 1, BankTransfer = 2, VNPay = 3
        PaidAt DATETIME NULL,
        BuyerId INT NOT NULL,
        DiscountId INT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Orders_Users FOREIGN KEY (BuyerId) REFERENCES Users(Id),
        CONSTRAINT FK_Orders_Discounts FOREIGN KEY (DiscountId) REFERENCES Discounts(Id)
    );
END
GO

-- Tạo bảng OrderDetails (Obsolete)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderDetails')
BEGIN
    CREATE TABLE OrderDetails (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL DEFAULT 0,
        Unit INT NOT NULL DEFAULT 1, -- Unit: Kg = 1, Box = 2, Piece = 3, etc.
        UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        ShippingFee DECIMAL(18,2) NOT NULL DEFAULT 0,
        TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        Subtotal DECIMAL(18,2) NOT NULL DEFAULT 0,
        PackagingStatus INT NOT NULL DEFAULT 0, -- OrderDetailStatus: Pending = 0, Packaging = 1, Shipped = 2, Delivered = 3, Cancelled = 4
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_OrderDetails_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id),
        CONSTRAINT FK_OrderDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
    );
END
GO

-- Tạo bảng OrderItems
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
BEGIN
    CREATE TABLE OrderItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL DEFAULT 0,
        Unit INT NOT NULL DEFAULT 1, -- Unit: Kg = 1, Box = 2, Piece = 3, etc.
        UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
        ShippingFee DECIMAL(18,2) NOT NULL DEFAULT 0,
        TaxAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
        Subtotal DECIMAL(18,2) NOT NULL DEFAULT 0,
        PackagingStatus INT NOT NULL DEFAULT 0, -- OrderItemStatus: Pending = 0, Packaging = 1, Shipped = 2, Delivered = 3, Cancelled = 4
        ShopId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id),
        CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
        CONSTRAINT FK_OrderItems_Stores FOREIGN KEY (ShopId) REFERENCES Stores(Id)
    );
END
GO

-- Tạo bảng OrderTimelines
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderTimelines')
BEGIN
    CREATE TABLE OrderTimelines (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        EventType INT NOT NULL, -- OrderEventType: OrderCreated = 0, OrderPaymentReceived = 1, ...
        Description NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_OrderTimelines_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id)
    );
END
GO

-- Tạo bảng Reviews
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE Reviews (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        ProductId INT NOT NULL,
        OrderId INT NOT NULL,
        Rating INT NOT NULL DEFAULT 5,
        Content NVARCHAR(500) NULL,
        ShopResponse NVARCHAR(500) NULL,
        ImageUrl NVARCHAR(255) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Reviews_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_Reviews_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
        CONSTRAINT FK_Reviews_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id)
    );
END
GO

-- Tạo bảng ChatRooms
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatRooms')
BEGIN
    CREATE TABLE ChatRooms (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NameRoom NVARCHAR(100) NOT NULL,
        Description NVARCHAR(255) NULL,
        BuyerId INT NOT NULL,
        SellerId INT NOT NULL,
        OrderId INT NULL,
        Type INT NOT NULL DEFAULT 0, -- ChatRoomType: ChatNormal = 0, Support = 1, Complaint = 2
        Status INT NOT NULL DEFAULT 0, -- ChatRoomStatus: InProgress = 0, Closed = 1, Resolved = 2
        LastMessage NVARCHAR(255) NULL,
        LastMessageTime DATETIME NULL DEFAULT GETDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_ChatRooms_Users_Buyer FOREIGN KEY (BuyerId) REFERENCES Users(Id),
        CONSTRAINT FK_ChatRooms_Users_Seller FOREIGN KEY (SellerId) REFERENCES Users(Id),
        CONSTRAINT FK_ChatRooms_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id)
    );
END
GO

-- Tạo bảng Chats
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Chats')
BEGIN
    CREATE TABLE Chats (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SenderId INT NOT NULL,
        ChatRoomId INT NOT NULL,
        Content NVARCHAR(MAX) NULL,
        ImageUrl NVARCHAR(255) NULL,
        Type INT NOT NULL DEFAULT 0, -- ChatMessageType: Text = 0, Image = 1, Complaint = 2
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Chats_Users FOREIGN KEY (SenderId) REFERENCES Users(Id),
        CONSTRAINT FK_Chats_ChatRooms FOREIGN KEY (ChatRoomId) REFERENCES ChatRooms(Id)
    );
END
GO

-- Tạo bảng Notifications
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
BEGIN
    CREATE TABLE Notifications (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Content NVARCHAR(255) NOT NULL,
        LinkUrl NVARCHAR(255) NULL,
        Type INT NOT NULL, -- NotificationType: Order = 1, Payment = 2, Discount = 3, ...
        IsRead BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng PaymentMethods
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PaymentMethods')
BEGIN
    CREATE TABLE PaymentMethods (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CardName NVARCHAR(100) NOT NULL,
        PaymentType INT NOT NULL DEFAULT 1, -- PaymentType: Bank = 1, Visa = 2, Mastercard = 3, ...
        AccountNumber NVARCHAR(50) NOT NULL,
        AccountHolderName NVARCHAR(100) NOT NULL,
        BankName NVARCHAR(100) NOT NULL,
        UserId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_PaymentMethods_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng BusinessRegistrations
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BusinessRegistrations')
BEGIN
    CREATE TABLE BusinessRegistrations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        BusinessType INT NOT NULL DEFAULT 1, -- StoreType: Farmer = 1, Retailer = 2, Wholesaler = 3
        Address NVARCHAR(255) NOT NULL,
        TaxCode NVARCHAR(50) NOT NULL,
        BusinessName NVARCHAR(100) NOT NULL,
        BusinessLicenseUrl NVARCHAR(255) NULL,
        Notes NVARCHAR(500) NULL,
        RegistrationStatus INT NOT NULL DEFAULT 0, -- RegistrationStatus: Pending = 0, Approved = 1, Rejected = 2, Cancelled = 3
        UserId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_BusinessRegistrations_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng RegistrationApprovalResults
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RegistrationApprovalResults')
BEGIN
    CREATE TABLE RegistrationApprovalResults (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        RegistrationId INT NOT NULL,
        AdminId INT NOT NULL,
        ApprovalResult INT NOT NULL, -- ApprovalResult: Approved = 1, Rejected = 2, Warning = 3, Pending = 4
        Note NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_RegistrationApprovalResults_BusinessRegistrations FOREIGN KEY (RegistrationId) REFERENCES BusinessRegistrations(Id),
        CONSTRAINT FK_RegistrationApprovalResults_Users FOREIGN KEY (AdminId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng Transactions
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Transactions')
BEGIN
    CREATE TABLE Transactions (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TransactionCode NVARCHAR(50) NOT NULL,
        OrderId INT NOT NULL,
        BuyerId INT NOT NULL,
        Amount DECIMAL(18,2) NOT NULL DEFAULT 0,
        PaymentMethod INT NOT NULL, -- PaymentMethodEnum: PaymentAfter = 1, BankTransfer = 2, VNPay = 3
        Status INT NOT NULL DEFAULT 0, -- TransactionStatus: Pending = 0, Success = 1, Failed = 2, Refunded = 3, Cancelled = 4
        PaymentDueDate DATETIME NULL,
        PaymentDate DATETIME NULL,
        Details NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Transactions_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id),
        CONSTRAINT FK_Transactions_Users FOREIGN KEY (BuyerId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng Carts
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Carts')
BEGIN
    CREATE TABLE Carts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Carts_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo bảng ShopCarts
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ShopCarts')
BEGIN
    CREATE TABLE ShopCarts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ShopId INT NOT NULL,
        CartId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_ShopCarts_Stores FOREIGN KEY (ShopId) REFERENCES Stores(Id),
        CONSTRAINT FK_ShopCarts_Carts FOREIGN KEY (CartId) REFERENCES Carts(Id)
    );
END
GO

-- Tạo bảng CartItems
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CartItems')
BEGIN
    CREATE TABLE CartItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL DEFAULT 1,
        ShopCartId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
        CONSTRAINT FK_CartItems_ShopCarts FOREIGN KEY (ShopCartId) REFERENCES ShopCarts(Id)
    );
END
GO

-- Tạo bảng ContactRequests
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ContactRequests')
BEGIN
    CREATE TABLE ContactRequests (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL,
        Subject NVARCHAR(200) NOT NULL,
        Message NVARCHAR(1000) NOT NULL,
        IsRead BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0
    );
END
GO
