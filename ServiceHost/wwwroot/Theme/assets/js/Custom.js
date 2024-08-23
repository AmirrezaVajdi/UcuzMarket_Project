const fullUrl = location.protocol + '//' + location.host;
const storageName = "cart-items";

async function SetDefaultAdderss(id) {
    var headersList = {
        "Content-Type": "application/json"
    }
    var data = { Id: id };
    var json = JSON.stringify(data);
    var response = await fetch(fullUrl + "/api/Delivery/SetDefaultAddress", {
        method: "POST",
        body: json,
        headers: headersList
    });

    if (window.location.href == (fullUrl + '/CheckOut')) {
        GetDefaultDelivery();
    }
}

function AddToCart(Id, name, slug, picture, price, PriceWithDiscount, discountRate) {

    let products = localStorage.getItem(storageName);

    if (products === null) {
        productsModel = [];
    }
    else {
        var productsModel = JSON.parse(products);
    }

    let count = document.getElementById("productCount").value;

    const currentProduct = productsModel.find(x => x.Id == Id);

    if (currentProduct !== undefined) {
        productsModel.find(x => x.Id === Id).count = parseInt(currentProduct.count) + parseInt(count);
    }
    else {
        if (PriceWithDiscount === undefined || PriceWithDiscount === "") {
            PriceWithDiscount = 0;
        }

        if (price === undefined || PriceWithDiscount === "") {
            price = 0;
        }

        count = parseInt(count);
        const product = {
            Id,
            name,
            slug,
            picture,
            price,
            count,
            PriceWithDiscount,
            discountRate
        }

        productsModel.push(product);
    }

    localStorage.setItem(storageName, JSON.stringify(productsModel));

    UpdateCart();
}

function UpdateCart() {
    let products = localStorage.getItem(storageName);

    products = JSON.parse(products);

    if (products === null) {
        products = [];
    }

    let prdouctCount = document.getElementById("cart_items_count").innerText = ToPersianNumber(products.length);

    document.getElementById("cart_items_wrapper").innerHTML = "";

    let cart_items_wrapper = document.getElementById("cart_items_wrapper");

    products.forEach(function (product) {
        let currentProductCount = product.count;
        const res =
            `
             <div class="d-flex align-items-center">
                    <a class="position-relative flex-shrink-0" href="${fullUrl}/Product/${product.slug}">
                    ${(product.discountRate != "0" ? '<span class="badge text-bg-danger position-absolute top-0 start-0 z-2 mt-0 ms-0">' + ToPersianNumber(product.discountRate) + ' %' + '</span>' : '')}
                    
                        <img src="${fullUrl}/ProductPictures/${product.picture}" style=" width:100px;height:100px">
                    </a>
                    <div class="w-100 ps-3">
                        <h5 class="fs-sm fw-medium lh-base mb-2">
                            <a class="hover-effect-underline" href="${fullUrl}/Product/${product.slug}">${product.name}</a>
                        </h5>
                        ${(product.PriceWithDiscount != 0 ? '<div class="h6 pb-1 mb-2" > ' + product.PriceWithDiscount + ' تومان  <del class="text-body-tertiary fs-sm fw-normal d-block">' + product.price + ' تومان' + '</del>   </div >' : '<div class="h6 pb-1 mb-2">' + product.price + ' تومان</div>')}


                        <div class="count-input rounded-pill">
                            <button onclick="DecrementProductCount(${product.Id})" type="button" class="btn btn-icon btn-sm" data-decrement="" aria-label="Decrement quantity" ${(product.count <= 1 ? 'disabled' : '')}>
                                <i class="ci-minus"></i>
                            </button>
                            <input min="1" type="number" class="form-control form-control-sm" value="${currentProductCount}" readonly="">
                            <button onclick="IncrementProductCount(${product.Id})" type="button" class="btn btn-icon btn-sm" data-increment="" aria-label="Increment quantity">
                                <i class="ci-plus"></i>
                            </button>
                        </div>
                        <div class="float-end">
                            <button onclick="RemoveFromCart(${product.Id})" type="button" class="btn-close fs-sm" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-sm"></button>
                        </div>
                    </div>
             </div >
            `
        cart_items_wrapper.innerHTML += res;
    });

    addScriptFilesToHtml();
}

function RemoveFromCart(id) {

    let products = localStorage.getItem(storageName);

    if (products === null) {
        productsM = [];
    }
    else {
        var productsM = JSON.parse(products);
    }

    let index = productsM.findIndex(function (x) {
        return x.Id == id;
    });

    productsM.splice(index, 1);

    localStorage.setItem(storageName, JSON.stringify(productsM));

    UpdateCart();
}

function RemoveFromCartPage(id) {
    RemoveFromCart(id);
    UpdateCart();
    LoadCartItems();
}

function ToPersianNumber(intNum) {
    var chash = String(intNum);
    if (chash === "" || chash === null) {
        return 0;
    }

    chash = chash.replace(',', '');

    chash = chash.replace(/\d/g, d => '٠١٢٣٤٥٦٧٨٩'[d]);

    return chash;
}

function ToEnglishNumber(intNum) {
    var chash = String(intNum);
    if (chash === "" || chash === null) {
        return 0;
    }

    chash = chash.replace('٬', '');

    chash = chash.replace(/[۰-۹]/g, d => '۰۱۲۳۴۵۶۷۸۹'.indexOf(d));

    return chash;
}

function LoadCartItems() {
    if (window.location.href == (fullUrl + '/Cart')) {

        let products = localStorage.getItem(storageName);

        let totalPriced = [];

        products = JSON.parse(products);

        if (products === null) {
            products = [];
        }

        document.getElementById("cart-items").innerHTML = "";

        let cartItems = document.getElementById("cart-items");

        products.forEach(function (product) {
            let result = `
     <!-- Item -->
                        <tr>
                            <td class="py-3 ps-0">
                                <div class="d-flex align-items-center">
                                    <a class="position-relative flex-shrink-0" href="${fullUrl}/Product/${product.slug}">
                                    ${(product.PriceWithDiscount != "0" ? '<span class="badge text-bg-danger position-absolute top-0 start-0">' + ToPersianNumber(product.discountRate) + '%' + '</span>' : "")}
                                        <img src="${fullUrl}/ProductPictures/${product.picture}" style="
    width: 100px;
    height: 100px;
">
                                    </a>
                                    <div class="ps-2 ps-xl-3">
                                        <h5 class="lh-sm mb-2">
                                            <a class="hover-effect-underline fs-sm fw-medium"
                                              href="${fullUrl}/Product/${product.slug}">
                                                ${product.name}
                                            </a>
                                        </h5>
                                         <ul class="list-unstyled gap-1 fs-xs mb-0">
                                                    <li class="d-xl-none">
                                                        <span class="text-body-secondary">قیمت:</span>
                                                    <span class="text-dark-emphasis fw-medium">
                                                    ${(product.PriceWithDiscount != "0" ? product.PriceWithDiscount + ' تومان' + '<del class="text-body-tertiary fw-normal d-block">' + product.price + ' تومان' + '</del>' : product.price + ' تومان')} 
                                                    </span>
                                                    </li>
                                                </ul>
        <div class="count-input rounded-pill d-md-none mt-3">
            <button onclick="DecrementProductCount(${product.Id})" type="button" class="btn btn-sm btn-icon" data-decrement=""
                aria-label="Decrement quantity" ${(product.count <= 1 ? 'disabled' : '')}>
                <i class="ci-minus"></i>
            </button>
            <input type="number" class="form-control form-control-sm" value="${product.count}"
                readonly="">
                <button onclick="IncrementProductCount(${product.Id})" type="button" class="btn btn-sm btn-icon" data-increment=""
                    aria-label="Increment quantity">
                    <i class="ci-plus"></i>
                </button>
        </div>
                                    </div >
                                </div >
                            </td >
                            <td class="h6 py-3 d-none d-xl-table-cell">
                                                 ${(product.PriceWithDiscount != "0" ? product.PriceWithDiscount + ' تومان' + '<del class="text-body-tertiary fs-sm fw-normal d-block">' + product.price + ' تومان' + '</del>' : product.price + ' تومان')} 
                            </td>
                            <td class="py-3 d-none d-md-table-cell">
                                <div class="count-input rounded-pill">
                                    <button onclick="DecrementProductCount(${product.Id})" type="button" class="btn btn-icon" data-decrement=""
                                            aria-label="Decrement quantity" ${(product.count <= 1 ? 'disabled' : '')}>
                                        <i class="ci-minus"></i>
                                    </button>
                                    <input min="1" type="number" class="form-control" value="${product.count}" readonly="">
                                    <button onclick="IncrementProductCount(${product.Id})"  type="button" class="btn btn-icon" data-increment=""
                                            aria-label="Increment quantity">
                                        <i class="ci-plus"></i>
                                    </button>
                                </div>
                            </td>
                            <td class="h6 py-3 d-none d-md-table-cell">
                            ${(product.PriceWithDiscount != 0 ? ToPersianNumber((ToEnglishNumber(product.PriceWithDiscount) * ToEnglishNumber(product.count))) + ' تومان' : ToPersianNumber(ToEnglishNumber(product.price) * ToEnglishNumber(product.count)) + ' تومان')
                }
                            </td >
    <td class="text-end py-3 px-0">
        <button onclick="RemoveFromCartPage(${product.Id})" type="button" class="btn-close fs-sm" data-bs-toggle="tooltip"
            data-bs-custom-class="tooltip-sm"></button>
    </td>
                        </tr >
    `
            cartItems.innerHTML += result;

            let pc = ToEnglishNumber(product.price);
            let pwd = ToEnglishNumber(product.PriceWithDiscount);
            let count = product.count;

            const price = {
                pc,
                pwd,
                count
            };

            totalPriced.push(price);
        })

        document.getElementById("totalProductCount").innerText = 'مجموع قیمت' + '(' + ToPersianNumber(products.length) + ' محصول' + ')';

        let totalPrice = 0;
        let savePrice = 0;
        totalPriced.forEach(function (price) {
            let cu = Number(price.pc) * Number(price.count);
            totalPrice += cu;
            //savePrice += (price.pwd === undefined || price.pwd == 0 ? 0 : price.pc - price.pwd);
            savePrice += (price.pwd == undefined || price.pwd == 0 ? 0 : (price.pc - price.pwd) * price.count);
        });

        document.getElementById("totalProductPrice").innerText = ToPersianNumber(totalPrice) + ' تومان';
        document.getElementById("savePrice").innerText = ToPersianNumber(savePrice) + ' تومان';

        document.getElementById("payToAmount").innerText = ToPersianNumber(totalPrice - savePrice) + ' تومان';
    }
}

function RemoveAllCarts() {
    localStorage.removeItem(storageName);
    UpdateCart();
    LoadCartItems();
}

async function GetCheckoutModel() {

    let storage = localStorage.getItem(storageName);
    let storageModel = [];

    if (storage == null) {
        storage = [];
    }
    else {
        storage = JSON.parse(storage);
    }

    let checkoutModels = [];

    storage.forEach(function (product) {
        let productId = product.Id;

        let count = product.count;

        const model = {
            productId,
            count
        }
        checkoutModels.push(model);
    })

    let json = JSON.stringify(checkoutModels);
    let headersList = {
        "Content-Type": "application/json"
    }
    let response = await fetch(fullUrl + "/api/product/GetProductsCheckout", {
        method: "POST",
        body: json,
        headers: headersList
    });

    let result = await response.json();


    document.getElementById("cart-items").innerHTML = "";
    let cartItems = document.getElementById("cart-items");
    let totalPriced = [];
    result.forEach(function (product) {
        let res = `
     <!-- Item -->
                        <tr>
                            <td class="py-3 ps-0">
                                <div class="d-flex align-items-center">
                                    <a class="position-relative flex-shrink-0" href="${fullUrl}/Product/${product.slug}">
                                    ${(product.priceWithDiscount != null ? '<span class="badge text-bg-danger position-absolute top-0 start-0">' + ToPersianNumber(product.discountRate) + '%' + '</span>' : "")}
                                        <img src="${fullUrl}/ProductPictures/${product.picture}" style="
    width: 100px;
    height: 100px;
">
                                    </a>
                                    <div class="ps-2 ps-xl-3">
                                        <h5 class="lh-sm mb-2">
                                            <a class="hover-effect-underline fs-sm fw-medium"
                                              href="${fullUrl}/Product/${product.slug}">
                                                ${product.name}
                                            </a>
                                        </h5>
                                         <ul class="list-unstyled gap-1 fs-xs mb-0">
                                                    <li class="d-xl-none">
                                                        <span class="text-body-secondary">قیمت:</span>
                                                    <span class="text-dark-emphasis fw-medium">
                                                    ${(product.priceWithDiscount != null ? product.priceWithDiscount + ' تومان' + '<del class="text-body-tertiary fw-normal d-block">' + (product.price == null ? 0 : product.price) + ' تومان' + '</del>' : (product.price == null ? 0 : product.price) + ' تومان')} 
                                                    </span>
                                                    </li>
                                                </ul>
                                    </div >
                                </div >
                            </td >
                            <td class="h6 py-3 d-none d-xl-table-cell">
                                                 ${(product.priceWithDiscount != null ? ToPersianNumber(product.priceWithDiscount) + ' تومان' + '<del class="text-body-tertiary fs-sm fw-normal d-block">' + (product.price == null ? 0 : ToPersianNumber(product.price)) + ' تومان' + '</del>' : (product.price == null ? 0 : ToPersianNumber(product.price)) + ' تومان')} 
                            </td>
                            <td>${ToPersianNumber(storage.find(function (p) { return p.Id == product.id }).count)}</td>
                            <td class="h6 py-3 d-none d-md-table-cell">
                            ${(product.priceWithDiscount != null ? ToPersianNumber((ToEnglishNumber(product.priceWithDiscount) * ToEnglishNumber(storage.find(function (p) { return p.Id == product.id }).count))) + ' تومان' : ToPersianNumber(ToEnglishNumber((product.price == null ? 0 : product.price)) * ToEnglishNumber(storage.find(function (p) { return p.Id == product.id }).count)) + ' تومان')
            }
                            </td >
                        </tr >
    `
        cartItems.innerHTML += res;

        let pc = ToEnglishNumber(product.price);
        let pwd = ToEnglishNumber(product.priceWithDiscount);
        let count = storage.find(function (p) { return p.Id == product.id }).count;

        const price = {
            pc,
            pwd,
            count
        };

        totalPriced.push(price);
    })
    document.getElementById("totalProductCount").innerText = 'مجموع قیمت' + '(' + ToPersianNumber(result.length) + ' محصول' + ')';

    let totalPrice = 0;
    let savePrice = 0;
    totalPriced.forEach(function (price) {
        let cu = Number(price.pc) * Number(price.count);
        totalPrice += cu;
        savePrice += (price.pwd === "undefined" ? 0 : (price.pc - price.pwd) * price.count);
    });

    document.getElementById("totalProductPrice").innerText = ToPersianNumber(totalPrice) + ' تومان';
    document.getElementById("savePrice").innerText = ToPersianNumber(savePrice) + ' تومان';

    document.getElementById("payToAmount").innerText = ToPersianNumber(totalPrice - savePrice) + ' تومان';

    GetDefaultDelivery();
}

function addScriptFilesToHtml() {
    var r = document.getElementById('themejs');
    if (r == null) {
        var s = document.createElement('script');
        s.setAttribute('src', '/Theme/assets/js/theme.min.js');
        s.setAttribute('id', 'themejs');
        document.body.appendChild(s);
    }
}

function IncrementProductCount(id) {
    let products = localStorage.getItem(storageName);

    if (products == null) {
        products = [];
    }
    else {
        products = JSON.parse(products);
    }
    let product = products.find(x => x.Id == id);

    if (product != null) {
        product.count += 1;
        localStorage.setItem(storageName, JSON.stringify(products));
        UpdateCart();
        LoadCartItems();
    }
}

function DecrementProductCount(id) {
    let products = localStorage.getItem(storageName);

    if (products == null) {
        products = [];
    }
    else {
        products = JSON.parse(products);
    }
    let product = products.find(x => x.Id == id);

    if (product != null) {
        product.count -= 1;
        localStorage.setItem(storageName, JSON.stringify(products));
        UpdateCart();
        LoadCartItems();
    }
}

async function GetDefaultDelivery() {
    let headersList = {
        "Content-Type": "application/json"
    }

    let response = await fetch(fullUrl + "/api/Delivery/GetDefaultAddress", {
        method: "GET",
        headers: headersList
    });

    let result = await response.text();

    if (result != '') {

        let model = JSON.parse(result);
        document.getElementById("defaultAddress").innerText = '';
        let res = document.getElementById("defaultAddress").innerText = 'میاندوآب ، ' + model.address + ' , ' + model.postalCode;
    }
    else {
        document.getElementById("defaultAddress").innerText = '';
        let res = document.getElementById("defaultAddress").innerText = 'لطفا ادرس جدیدی را ثبت کنید';
    }
}