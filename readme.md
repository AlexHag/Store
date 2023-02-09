# Store
```
cd server
dotnet restore
dotnet ef database update
dotnet run
```
```
cd client
npm i
npm start
```

- Seed bogus data
- Write tests
- Automated testing pipeline, wrecker or github actions for CI
- View products ``[get]``
- Search products and sort categories ``[post]``
- Pagination
- Click on products and view details ``[get]``
- Click on products store and view products from that store ``[get]``
- Store owner can upload products ``[post]``
- Store owner can view own products ``[get]``
- Store owner can delete own products ``[delete]``
- Store owner can edit own products ``[put]``
- Add products to cart
- Delete products from cart
- Create microservice for purchasing products
- Admin page and privlages