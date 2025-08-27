# 🥗 NutraClinic – NTier Architecture & RESTful API Tabanlı Diyetisyen Randevu Sistemi
NutraClinic, modern kullanıcı arayüzü, güçlü mimarisi ve dinamik veri yapısıyla geliştirilmiş bir diyetisyen randevu yönetim sistemidir. Proje, **RESTful Web API**, ASP.NET Core MVC **(NTier Architecture)**, **JWT tabanlı kimlik doğrulama** ve ViewComponent destekli dinamik modüllerle yapılandırılmıştır.

Kullanıcılar, sistemde kayıtlı diyetisyenlerin müsait takviminden randevu talebinde bulunabilir; sekreterler bu talepleri onaylayabilir veya reddedebilir. Diyetisyenler ise yalnızca onaylanmış randevuları takvimlerinde görebilir ve kendi profillerini yönetebilir.

-----


## 💡 Projenin Öne Çıkan Özellikleri


### 🔗 RESTful Web API Entegrasyonu

- Tüm veri işlemleri (GET, POST, PUT, DELETE), ayrı katmanlı RESTful API aracılığıyla gerçekleştirilir.

- MVC arayüzü, HttpClient üzerinden API ile entegre şekilde çalışır.

- API, NTier mimaride, iş katmanına bağlı olarak SQL Server veritabanı üzerinde işlem yapar.

### 🧱 NTier (Katmanlı) Mimari

- Entities, Data Access, Business, API ve MVC UI olmak üzere çok katmanlı yapı.

- Kodun sürdürülebilirliğini ve okunabilirliğini artırmak için katmanlar ayrıştırılmıştır.

### 🛡️ JWT Authentication

- Kullanıcı ve personel girişi işlemlerinde JWT (JSON Web Token) tabanlı kimlik doğrulama kullanılır.

- Rol bazlı erişim kontrolü (User, Dietitian, Secretary).

Her kullanıcıya özel token ile güvenli istekler yapılır.

### 🔄 jQuery + AJAX ile Dinamik Veri Akışı

- Randevu takvimi gibi dinamik yapılar AJAX çağrıları ve fetch API üzerinden asenkron şekilde yönetilir.

- Sayfa yenilemeden form gönderimi, modal işlemleri, anlık randevu durumları gibi işlemler jQuery desteklidir.

### 🧩 ViewComponent ile Modülerlik

- Navbar, randevu kartları ve kullanıcıya özel bileşenler ViewComponent yapısıyla modülerleştirilmiştir.

- Kod tekrarını azaltır, UI tarafında bakım kolaylığı sağlar.


### 🔎 Area Yapısı
- Uygulamada farklı roller (User, Dietitian, Secretary) için sayfa ve controller ayrımı sağlar.

-----


## 🗓️ Uygulama Akışı

### 👤 Kullanıcı

- Diyetisyen listesinden bir uzman seçerek 1 aylık takvimindeki boş saatleri görür.

- Müsait bir saat için randevu talebi oluşturur.

- Talep, sekreter onaylayana kadar "Pending" olarak görünür.

### 🧑‍💼 Sekreter Paneli

- Diyetisyen ve Specialty (uzmanlık) yönetimi: ekleme, güncelleme, silme.

- Tüm gelen randevu taleplerini onaylama veya reddetme.

- Rol bazlı içerik yönetimi.

### 👩‍⚕️ Diyetisyen Paneli

- Profil bilgilerini güncelleyebilir.

- Sadece onaylanmış randevuları kendi takviminde görebilir.


-----


## 🛠️ Kullanılan Teknolojiler


#### Ntier Architecture (Katmanlı Mimari)
 Uygulama sunum, iş mantığı, veri erişimi ve veri katmanlarına ayrılmıştır. Bakım, test ve yeniden kullanılabilirlik açısından avantaj sağlar.
  
#### ASP.NET Core Web API
 RESTful mimariye uygun şekilde veri işlemleri bu katmanda gerçekleştirilir.

#### JWT (JSON Web Token)
  Kimlik doğrulama ve rol tabanlı yetkilendirme işlemleri için kullanılır.

#### ASP.NET Core 7
  Projenin temelini oluşturan modern ve modüler web çatısıdır.

#### Microsoft SQL Server
  Tüm verilerin saklandığı ilişkisel veritabanı sistemidir.

#### ViewComponent
  UI tarafında tekrar kullanılabilir, modüler bileşenlerin geliştirilmesini sağlar.

#### Area Yapısı
  Uygulamada farklı roller (User, Dietitian, Secretary) için sayfa ve controller ayrımı sağlar.

#### jQuery + AJAX
  Sayfa yenilemeden veri alma ve gönderme işlemleri için kullanılır.

#### Bootstrap 5
  Kullanıcı arayüzünün mobil uyumlu ve modern görünmesini sağlayan CSS framework’tür.



-----



## 🖼️ Ekran Görüntüleri & GIF’ler

##### GIF animasyonları birkaç saniyede görüntülenebilir.


### 👤User Section


#### Home 

<img src="https://github.com/user-attachments/assets/d07a65f2-1c30-4b21-bdfb-148c11e75535" width="700"  />


#### Departments

<img src="https://github.com/user-attachments/assets/412690da-71f8-429e-ad5f-6b4deb1675d4" width="700"  />


#### Dietitians

<img src="https://github.com/user-attachments/assets/3a245083-f5ef-45a4-a254-7f7dd33b1650" width="700"  />


#### Dietitian Information

<img src="https://github.com/user-attachments/assets/ad4baaab-e065-4bb9-8beb-82c475506e5b" width="700"  />



#### Appointment Section

<img src="https://github.com/user-attachments/assets/f8b86b1b-bec9-4123-ac96-2fdda2fbe540" width="700"  />



#### Appointment Section - Available Hours

<img src="https://github.com/user-attachments/assets/8e40b188-5a53-470b-8082-f7a14827ed15" width="700"  />



####  My Appointments 

<img src="https://github.com/user-attachments/assets/1ce5d201-1303-4f1f-888e-b03577cce8d4" width="700"  />

<img src="https://github.com/user-attachments/assets/51992630-cb8c-4600-ada1-3455550a1066" width="700"  />





