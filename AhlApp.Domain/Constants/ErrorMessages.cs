namespace AhlApp.Domain.Constants
{
    public static class ErrorMessages
    {
        // Kullanıcı Hata Mesajları
        public const string EmailAlreadyExists = "Bu e-posta zaten kayıtlı.";
        public const string UserNotFound = "Kullanıcı bulunamadı.";
        public const string InvalidCredentials = "Geçersiz e-posta veya şifre.";
        public const string NoAccountsFoundForUser = "Kullanıcıya ait hesap bulunamadı.";
        public const string FailedToRetrieveUserDetails = "Kullanıcı bilgileri alınamadı.";


        // Hesap Hata Mesajları
        public const string AccountNotFound = "Hesap bulunamadı.";
        public const string AccountNumberAlreadyExists = "Bu hesap numarası zaten mevcut.";
        public const string NoAccessToAccount = "Bu hesaba erişim izniniz yok.";
        public const string NoAccessToSenderAccount = "Gönderici hesaba erişim izniniz yok.";

        // İşlem (Transaction) Hata Mesajları
        public const string NoTransactionsFoundForDateRange = "Belirtilen tarih aralığı için işlem bulunamadı.";
        public const string NoTransactionsFoundForForecast = "Tahmin oluşturmak için yeterli işlem bulunamadı.";
        public const string NegativeAmountNotAllowed = "Negatif bir tutar işlem yapılamaz.";
        public const string DepositAmountMustBePositive = "Yatırma işlemi için tutar pozitif olmalıdır.";
        public const string WithdrawalAmountMustBePositive = "Çekim işlemi için tutar pozitif olmalıdır.";
        public const string DifferentCurrenciesNotAllowed = "Farklı para birimleriyle işlem yapılamaz.";
        public const string TransferAmountMustBePositive = "Transfer miktarı pozitif olmalıdır.";
        public const string InvalidDateRangeType = "Geçersiz tarih aralığı türü.";
        public const string InvalidTransactionType = "Geçersiz işlem türü.";

        // Gönderici ve Alıcı Hesap Hata Mesajları
        public const string SenderAccountNotFound = "Gönderici hesap bulunamadı.";
        public const string ReceiverAccountNotFound = "Alıcı hesap bulunamadı.";
        public const string InsufficientBalanceInSenderAccount = "Gönderici hesapta yeterli bakiye bulunmuyor.";

        // Kategori Hata Mesajları
        public const string CategoryNotFound = "Kategori bulunamadı.";
        public const string InvalidCategory = "Geçersiz kategori.";
        public const string CategoryNameCannotBeEmpty = "Kategori adı boş olamaz.";
        public const string DepositCategoryNotFound = "'Deposit' kategorisi bulunamadı.";
        public const string TransferCategoryNotFound = "'Hesaptan Hesaba Transfer' kategorisi bulunamadı.";

        // Para Birimi (Currency) Hata Mesajları
        public const string CurrencyCannotBeNull = "Para birimi boş veya null olamaz.";

        // Hesap Kimlik Hata Mesajları
        public const string AccountIdRequiredForExpenseOrDeposit = "Gider veya para yatırma işlemleri için hesap kimliği gereklidir.";
        public const string AccountIdsRequiredForTransfer = "Transfer işlemleri için hem gönderici hem de alıcı hesap kimlikleri gereklidir.";

        // Bakiye (Balance) Hata Mesajları
        public const string InsufficientFunds = "Yetersiz bakiye.";

        // JWT Hata Mesajları
        public const string TokenMissing = "Kullanıcı doğrulanamadı. Token eksik.";
        public const string InvalidToken = "Kullanıcı doğrulanamadı. Geçersiz token.";
        public const string TokenExpiredOrInvalid = "Kullanıcı doğrulanamadı. Token geçersiz veya süresi dolmuş.";
    }
}
