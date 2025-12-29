
# Subscription Box Management Platform


Bu proje, **abonelik tabanlı e-ticaret sistemleri** için geliştirilmiş, **ölçeklenebilir ve modüler** bir backend uygulamasıdır.
Hem **tek seferlik siparişleri** hem de **abonelik yenilemelerini** destekler; stok takibi, faturalama, kişiselleştirme ve arka plan işlemlerini kapsar.


Bu proje;
* Müşteri CRUD işlemleri, Subscription, Id, Email'e göre müşteri listeleme ayrıca tüm müşterileri listeleme.
* Abonelik oluşturma, abonelik yenileme ve faturalama periyotları
* Tek seferlik ve abonelik bazlı sipariş oluşturma
* Kargo durumu takibi ve kargo numarası alma
* Fatura yönetimi, ödeme durumu takibi
* Gerçek zamanlı stok düşümü, stok ekleme ve güncelleme, background worker ile lowStock takibi
* Kişiselleştirme özelliği ile kullanıcıya tercihleri doğrultusunda ürün önerisi ve paket seçimi
* Tekil sipariş oluşturulunca, ilk abonelik başlangıçında ve abonelik yenilemelerinde kullanıcıya mail gönderimi
* Jwt ve role-based ile kimlik doğrulama ve yetkilendirme işlemlerini içermektedir.




## Kurulum & Çalıştırma

### 🔹 Projeyi Klonlayın

```bash
git clone https://github.com/kullanici-adi/subscription-platform.git
cd subscription-platform
```

---

### 🔹 Konfigürasyon

#### 🔹 PostgreSQL Veritabanı Oluşturma

```json
CREATE DATABASE SubscriptionPlatformDb;
```


#### 🔹 Veritabanı Ayarı (`appsettings.json`)

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=SubscriptionDb;Username=postgres;Password=1234"
}
```

---

#### 🔹 JWT Ayarları

```json
"Jwt": {
  "Key": "SUPER_SECRET_KEY",
  "Issuer": "SubscriptionPlatform",
  "Audience": "SubscriptionPlatformUsers",
  "ExpireMinutes": 60
}
```

---

#### 🔹 Mail Ayarları (Mailtrap)

```json
"MailSettings": {
  "Host": "smtp.mailtrap.io",
  "Port": 587,
  "UserName": "MAILTRAP_USER",
  "Password": "MAILTRAP_PASS",
  "From": "noreply@subscription.com"
}
```

---

### 🔹 Veritabanı Migration

```bash
dotnet ef database update --project SubscriptionPlatform.Infrastructure --startup-project SubscriptionPlatform.API
```

---

### 🔹 Uygulamayı Çalıştırın

```bash
dotnet run --project SubscriptionPlatform.API
```

Swagger arayüzü:

```
https://localhost:5161/swagger
```



