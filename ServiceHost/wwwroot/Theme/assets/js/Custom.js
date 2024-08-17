const fullUrl = location.protocol + '//' + location.host;
const storageName = "cart-items";
const dayToCookeLife = 10;
const Pn = ["۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹"];
const En = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9"];

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
}

function AddToCart(Id, name, slug, picture, price, PriceWithDiscount) {

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

        const product = {
            Id,
            name,
            slug,
            picture,
            price,
            count,
            PriceWithDiscount
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
        const res =
            `
             <div class="d-flex align-items-center">
                    <a class="position-relative flex-shrink-0" href="${fullUrl}/Product/${product.slug}">
                <span class="badge text-bg-danger position-absolute top-0 start-0 z-2 mt-0 ms-0">${ToPersianNumber(product.count)}</span>
                        <img src="${fullUrl}/ProductPictures/${product.picture}" style=" width:100px;height:100px">
                    </a>
                    <div class="w-100 ps-3">
                        <h5 class="fs-sm fw-medium lh-base mb-2">
                            <a class="hover-effect-underline" href="${fullUrl}/Product/${product.slug}">${product.name}</a>
                        </h5>
                        <div class="h6 pb-1 mb-2">${product.price} تومان</div>
                        <div class="float-end">
                            <button onclick="RemoveFromCart(${product.Id})" type="button" class="btn-close fs-sm" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-sm" data-bs-title="حذف" aria-label="حذف"></button>
                        </div>
                    </div>
             </div >
            `
        cart_items_wrapper.innerHTML += res;
    });
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

function ToPersianNumber(intNum) {
    var chash = String(intNum);
    if (chash === "" || chash === null) {
        return intnum;
    }
    for (let i = 0; i < 10; i++) {
        chash = chash.replace(En[i], Pn[i]);
    }
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
                                    ${(product.PriceWithDiscount != "0" ? '<span class="badge text-bg-danger position-absolute top-0 start-0">' + 'درصد تخفیف' + '</span>' : "")}
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
            <button type="button" class="btn btn-sm btn-icon" data-decrement=""
                aria-label="Decrement quantity">
                <i class="ci-minus"></i>
            </button>
            <input type="number" class="form-control form-control-sm" value="${product.count}"
                readonly="">
                <button type="button" class="btn btn-sm btn-icon" data-increment=""
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
                                    <button type="button" class="btn btn-icon" data-decrement=""
                                            aria-label="Decrement quantity">
                                        <i class="ci-minus"></i>
                                    </button>
                                    <input type="number" class="form-control" value="${product.count}" readonly="">
                                    <button type="button" class="btn btn-icon" data-increment=""
                                            aria-label="Increment quantity">
                                        <i class="ci-plus"></i>
                                    </button>
                                </div>
                            </td>
                            <td class="h6 py-3 d-none d-md-table-cell">
                            ${(product.PriceWithDiscount != "0" ? "" : "")}
                            </td>
                            <td class="text-end py-3 px-0">
                                <button type="button" class="btn-close fs-sm" data-bs-toggle="tooltip"
                                        data-bs-custom-class="tooltip-sm" data-bs-title="Remove"
                                        aria-label="Remove from cart"></button>
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
    totalPriced.forEach(function (price) {
        let cu = Number(price.pc) * Number(price.count);
        totalPrice += cu;
    });

    document.getElementById("totalProductPrice").innerText = String(ToPersianNumber(totalPrice));
}