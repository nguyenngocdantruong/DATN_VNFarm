-- Tạo cơ sở dữ liệu VNFarm
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VNFarm')
BEGIN
    CREATE DATABASE VNFarm;
END
GO

USE VNFarm;
GO

-- Bảng User
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
    
    -- Đảm bảo Email là duy nhất
    CREATE UNIQUE INDEX IX_Users_Email ON Users(Email) WHERE IsDeleted = 0;
END
GO

-- Bảng Store
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
        BusinessType INT NOT NULL DEFAULT 0, -- StoreType: Farmer = 0, Company = 1
        IsActive BIT NOT NULL DEFAULT 1,
        VerificationStatus INT NOT NULL DEFAULT 0, -- StoreStatus: Pending = 0, Verified = 1, Rejected = -1
        AverageRating FLOAT NOT NULL DEFAULT 5.0,
        ReviewCount INT NOT NULL DEFAULT 0,
        UserId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Stores_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Bảng Category
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categories')
BEGIN
    CREATE TABLE Categories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        MinPrice INT NOT NULL DEFAULT 0,
        MaxPrice INT NOT NULL DEFAULT 0,
        Description NVARCHAR(500) NULL,
        IconUrl NVARCHAR(255) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0
    );
END
GO

-- Bảng Product
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        Price INT NOT NULL,
        ImageUrl NVARCHAR(255) NULL,
        StockQuantity INT NOT NULL DEFAULT 0,
        Unit INT NOT NULL DEFAULT 1, -- Unit: Kg = 1, Box = 2, Piece = 3...
        SoldQuantity INT NOT NULL DEFAULT 0,
        StoreId INT NOT NULL,
        CategoryId INT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        Origin NVARCHAR(100) NULL,
        AverageRating FLOAT NOT NULL DEFAULT 0,
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

-- Bảng Discount
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Discounts')
BEGIN
    CREATE TABLE Discounts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(50) NOT NULL,
        Description NVARCHAR(500) NULL,
        RemainingQuantity INT NOT NULL DEFAULT 0,
        Status INT NOT NULL DEFAULT 0, -- DiscountStatus: Active = 0, Inactive = 1
        StartDate DATETIME NOT NULL,
        EndDate DATETIME NOT NULL,
        Type INT NOT NULL DEFAULT 0, -- DiscountType: Percentage = 0, FixedAmount = 1, FreeShipping = 2
        DiscountAmount INT NOT NULL DEFAULT 0,
        MinimumOrderAmount INT NOT NULL DEFAULT 0,
        MaximumDiscountAmount INT NOT NULL DEFAULT 0,
        StoreId INT NULL, -- Null nếu là mã giảm giá toàn hệ thống
        UserId INT NULL, -- Null nếu là mã giảm giá toàn hệ thống
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Discounts_Stores FOREIGN KEY (StoreId) REFERENCES Stores(Id),
        CONSTRAINT FK_Discounts_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Bảng Order
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Orders')
BEGIN
    CREATE TABLE Orders (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderCode NVARCHAR(50) NOT NULL,
        Status INT NOT NULL DEFAULT 0, -- OrderStatus: Pending = 0, Processing = 1...
        Notes NVARCHAR(500) NULL,
        TotalAmount INT NOT NULL DEFAULT 0,
        ShippingFee INT NOT NULL DEFAULT 0,
        TaxAmount INT NOT NULL DEFAULT 0,
        DiscountAmount INT NOT NULL DEFAULT 0,
        FinalAmount INT NOT NULL DEFAULT 0,
        OrderPaymentId BIGINT NOT NULL DEFAULT 0,
        PaymentStatus INT NOT NULL DEFAULT 1, -- PaymentStatus: Unpaid = 1, PartiallyPaid = 2...
        PaymentMethod INT NOT NULL DEFAULT 2, -- PaymentMethodEnum: PaymentAfter = 1, BankTransfer = 2...
        PaidAt DATETIME NULL,
        TrackingNumber NVARCHAR(50) NULL,
        ShippingMethod NVARCHAR(50) NULL,
        ShippingPartner NVARCHAR(50) NULL,
        ShippedAt DATETIME NULL,
        DeliveredAt DATETIME NULL,
        CancelledAt DATETIME NULL,
        ShippingName NVARCHAR(100) NULL,
        ShippingPhone NVARCHAR(20) NULL,
        ShippingAddress NVARCHAR(255) NULL,
        ShippingProvince NVARCHAR(100) NULL,
        ShippingDistrict NVARCHAR(100) NULL,
        ShippingWard NVARCHAR(100) NULL,
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

-- Bảng OrderItem
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderItems')
BEGIN
    CREATE TABLE OrderItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL DEFAULT 1,
        Unit INT NOT NULL DEFAULT 1, -- Unit: Kg = 1, Box = 2, Piece = 3...
        UnitPrice INT NOT NULL DEFAULT 0,
        ShippingFee INT NOT NULL DEFAULT 0,
        TaxAmount INT NOT NULL DEFAULT 0,
        Subtotal INT NOT NULL DEFAULT 0,
        PackagingStatus INT NOT NULL DEFAULT 0, -- OrderItemStatus: Pending = 0, Processing = 1...
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

-- Bảng OrderTimeline
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderTimelines')
BEGIN
    CREATE TABLE OrderTimelines (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderId INT NOT NULL,
        OrderId1 INT NULL, -- Thêm cột này để khắc phục lỗi shadow property
        EventType INT NOT NULL DEFAULT 0, -- OrderEventType: OrderCreated = 0, OrderPaymentReceived = 1...
        Status INT NOT NULL DEFAULT 0, -- OrderTimelineStatus: Pending = 0, Completed = 1, Cancelled = 2
        Description NVARCHAR(500) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_OrderTimelines_Orders FOREIGN KEY (OrderId) REFERENCES Orders(Id)
    );
END
GO

-- Bảng Review
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

-- Bảng Cart
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

-- Bảng ShopCart
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

-- Bảng CartItem
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

-- Bảng ChatRoom
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatRooms')
BEGIN
    CREATE TABLE ChatRooms (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        NameRoom NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL,
        BuyerId INT NOT NULL,
        SellerId INT NOT NULL,
        OrderId INT NULL,
        Type INT NOT NULL DEFAULT 1, -- ChatRoomType: DisputeByShipping = 0, ChatNormal = 1...
        Status INT NOT NULL DEFAULT 0, -- ChatRoomStatus: InProgress = 0, Closed = 1, Solved = 2
        LastMessage NVARCHAR(500) NULL,
        LastMessageTime DATETIME NULL,
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

-- Bảng Chat
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Chats')
BEGIN
    CREATE TABLE Chats (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SenderId INT NOT NULL,
        ChatRoomId INT NOT NULL,
        Content NVARCHAR(MAX) NULL,
        ImageUrl NVARCHAR(255) NULL,
        Type INT NOT NULL DEFAULT 0, -- ChatMessageType: Text = 0, Image = 1
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Chats_Users FOREIGN KEY (SenderId) REFERENCES Users(Id),
        CONSTRAINT FK_Chats_ChatRooms FOREIGN KEY (ChatRoomId) REFERENCES ChatRooms(Id)
    );
END
GO

-- Bảng Notification
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
BEGIN
    CREATE TABLE Notifications (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        Content NVARCHAR(500) NOT NULL,
        LinkUrl NVARCHAR(255) NULL,
        Type INT NOT NULL, -- NotificationType: Order = 1, Payment = 2...
        IsRead BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

-- Tạo index để tối ưu query
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_OrderCode')
BEGIN
    CREATE INDEX IX_Orders_OrderCode ON Orders(OrderCode);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Orders_BuyerId')
BEGIN
    CREATE INDEX IX_Orders_BuyerId ON Orders(BuyerId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrderItems_OrderId')
BEGIN
    CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_StoreId')
BEGIN
    CREATE INDEX IX_Products_StoreId ON Products(StoreId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Products_CategoryId')
BEGIN
    CREATE INDEX IX_Products_CategoryId ON Products(CategoryId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_OrderTimelines_OrderId')
BEGIN
    CREATE INDEX IX_OrderTimelines_OrderId ON OrderTimelines(OrderId);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Discounts_Code')
BEGIN
    CREATE INDEX IX_Discounts_Code ON Discounts(Code);
END
GO

-- Kiểm tra và thêm cột OrderId1 vào bảng OrderTimelines nếu chưa có
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'OrderTimelines')
    AND NOT EXISTS (SELECT * FROM sys.columns WHERE name = 'OrderId1' AND object_id = OBJECT_ID('OrderTimelines'))
BEGIN
    ALTER TABLE OrderTimelines ADD OrderId1 INT NULL;
END
GO

PRINT 'Database created successfully!'
GO
