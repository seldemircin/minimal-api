# .NET Minimal API Eğitim Projesi

Bu proje, .NET Minimal API'nin temel özelliklerini ve en iyi uygulamalarını göstermek için hazırlanmış bir eğitim projesidir. Her bir bölüm, modern web API geliştirmede önemli bir konuyu ele almaktadır.

## 🚀 Özellikler

- Minimal API temelleri
- Route handlers ve parametreler
- HTTP durum kodları
- Global hata yönetimi
- CORS yapılandırması
- Veri doğrulama
- Bağımlılık enjeksiyonu
- Repository pattern
- AutoMapper entegrasyonu
- Entity Framework Core
- ASP.NET Core Identity
- JWT authentication

## 📋 Bölümler

1. **Hello World (ch_01_Hello)**
   - Minimal API'ye giriş
   - Temel endpoint yapısı
   - Response modelleme

2. **Route Handlers (ch_02_RouteHandlers)**
   - Farklı handler türleri
   - Lambda expressions
   - Local functions

3. **Route Parameters (ch_03_RouteParameters)**
   - Route parametreleri
   - Query string parametreleri
   - Model binding

4. **Status Codes (ch_04_StatusCodes)**
   - HTTP durum kodları
   - Results sınıfı
   - Response yönetimi

5. **Global Error Handler (ch_05_GlobalErrorHandler)**
   - Exception handling
   - Custom exceptions
   - Error response modelleme

6. **CORS (ch_06_Cors)**
   - CORS yapılandırması
   - Policy tanımları
   - Middleware kullanımı

7. **Validation (ch_07_Validation)**
   - Data annotations
   - Model validation
   - Custom validation

8. **Dependency Injection (ch_08_DependencyInjection)**
   - Service registration
   - Constructor injection
   - Lifetime management

9. **DI with Interfaces (ch_09_DI_Interfaces)**
   - Interface-based programming
   - Loose coupling
   - Service abstraction

10. **Data Access Layer (ch_10_dal)**
    - Entity Framework Core
    - DbContext yapılandırması
    - Repository pattern

11. **Repository in Use (ch_11_repo_in_use)**
    - Generic repository
    - CRUD operasyonları
    - Service layer

12. **AutoMapper (ch_12_auto_mapper)**
    - DTO pattern
    - Object mapping
    - Profile yapılandırması

13. **Configuration (ch_13_configuration)**
    - Extension methods
    - Middleware yapılandırması
    - Service configuration

14. **Relations (ch_14_relations)**
    - Entity ilişkileri
    - Eager loading
    - Navigation properties

15. **Identity (ch_15_identity)**
    - ASP.NET Core Identity
    - User management
    - Role-based authorization

16. **JWT (ch_16_jwt)**
    - JWT authentication
    - Token management
    - Refresh token

## 🛠️ Teknolojiler

- .NET 7.0
- Entity Framework Core
- MySQL
- AutoMapper
- ASP.NET Core Identity
- JWT

## 📦 Kurulum

1. Projeyi klonlayın:
```bash
git clone https://github.com/yourusername/minimal-api.git
```

2. Proje dizinine gidin:
```bash
cd minimal-api
```

3. Bağımlılıkları yükleyin:
```bash
dotnet restore
```

4. Veritabanını oluşturun:
```bash
dotnet ef database update
```

5. Uygulamayı çalıştırın:
```bash
dotnet run
```

## 🔧 Yapılandırma

`appsettings.json` dosyasında aşağıdaki ayarları yapılandırın:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  },
  "JwtSettings": {
    "SecretKey": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience"
  }
}
```

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Daha fazla bilgi için `LICENSE` dosyasına bakın.

## 👥 Katkıda Bulunma

1. Bu depoyu fork edin
2. Feature branch'i oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'Add some amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📧 İletişim

Proje Sahibi - [@yourusername](https://github.com/yourusername)

Proje Linki: [https://github.com/yourusername/minimal-api](https://github.com/yourusername/minimal-api) 