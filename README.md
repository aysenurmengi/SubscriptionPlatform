
# 📦 Subscription Box Management Platform


Bu proje, **abonelik tabanlı e-ticaret sistemleri** için geliştirilmiş, **ölçeklenebilir ve modüler** bir backend uygulamasıdır.
Hem **tek seferlik siparişleri** hem de **abonelik yenilemelerini** destekler; stok takibi, faturalama, kişiselleştirme ve arka plan işlemlerini kapsar.


## 🚀 Özellikler

### 👤 Müşteri Yönetimi

* Müşteri CRUD işlemleri
* ID veya e-posta ile müşteri getirme
* Aktif aboneliği olan müşterileri listeleme
* Bir müşterinin birden fazla aboneliği olabilir

---

### 🔁 Abonelik Yönetimi

* Abonelik entity yapısı
* Faturalama periyotları (Aylık / Yıllık)
* `NextRenewalDate` (Bir sonraki yenileme tarihi)
* Abonelik durum takibi
* Manuel yenileme desteği

---

### 🛒 Sipariş Yönetimi

* Tek seferlik siparişler
* Abonelik bazlı siparişler
* Sipariş kalemleri (OrderItem)
* Kargo durumu takibi
* Kargo takip numarası desteği
* Abonelik / normal sipariş için ayrı iş akışları

---

### 🧾 Fatura Yönetimi

* Invoice entity
* Ödeme durumu takibi

---

### 📦 Stok (Inventory) Yönetimi

* Inventory entity
* Gerçek zamanlı stok düşümü
* Düşük stok eşiği (Low stock threshold)
* Stok kontrolü yapan background worker
* (Planlandı) Düşük stok uyarı sistemi

---

### 🎯 Kişiselleştirme (Personalization)

* Kullanıcı tercihleri (Preference entity)
* Etiket (tag) bazlı eşleştirme
* Tercihlere göre ürün belirleme

---

### 📧 E-Posta Sistemi

* Sipariş oluşturulunca mail gönderimi
* Sipariş türüne göre farklı mail şablonları
* Mailtrap entegrasyonu

---

### ⚙️ Arka Plan İşlemleri (Background Processing)

* Stok izleme worker’ı
* Periyodik stok kontrolü

---

### 🔐 Kimlik Doğrulama & Yetkilendirme

* JWT Bearer Authentication
* Rol bazlı yetkilendirme

  * Admin
  * Customer
  * Fulfillment

---

## 🛠️ Teknoloji Yığını

* **Backend:** ASP.NET Core
* **Mimari:** Clean Architecture + CQRS
* **Veritabanı:** PostgreSQL
* **ORM:** Entity Framework Core
* **Authentication:** JWT
* **Background Jobs:** Hosted Services
* **Mail:** SMTP (Mailtrap)

---

## 📋 Gereksinimler

Projeyi çalıştırmadan önce sisteminizde şunlar bulunmalıdır:

* .NET SDK **8+**
* PostgreSQL veya MSSQL
* Git
* Mailtrap hesabı (mail testleri için)

---

## 🚀 Kurulum & Çalıştırma

### 1️⃣ Projeyi Klonlayın

```bash
git clone https://github.com/kullanici-adi/subscription-platform.git
cd subscription-platform
```

---

### 2️⃣ Konfigürasyon

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

### 3️⃣ Veritabanı Migration

```bash
dotnet ef database update --project SubscriptionPlatform.Infrastructure --startup-project SubscriptionPlatform.API
```

---

### 4️⃣ Uygulamayı Çalıştırın

```bash
dotnet run --project SubscriptionPlatform.API
```

Swagger arayüzü:

```
https://localhost:5161/swagger
```



## 🔒 Güvenlik

* JWT Authentication
* Rol bazlı yetkilendirme
* Güvenli parola hashleme
* API seviyesinde authorization
