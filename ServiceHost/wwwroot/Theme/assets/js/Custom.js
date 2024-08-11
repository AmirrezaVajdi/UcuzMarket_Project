const fullUrl = location.protocol + '//' + location.host;
const storageName = "cart-items";
const dayToCookeLife = 10;
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

    updateCart();
}

function updateCart() {
    let products = localStorage.getItem(storageName);

    products = JSON.parse(products);

    if (products === null) {
        products = [];
    }

    let prdouctCount = document.getElementById("cart_items_count").innerText = products.length;

    document.getElementById("cart_items_wrapper").innerHTML = "";

    let cart_items_wrapper = document.getElementById("cart_items_wrapper");

    products.forEach(function (product) {
        const res =
            `
             <div class="d-flex align-items-center">
                    <a class="position-relative flex-shrink-0" href="${fullUrl}/Product/${product.slug}">
                <span class="badge text-bg-danger position-absolute top-0 start-0 z-2 mt-0 ms-0">-${product.PriceWithDiscount}</span>
                        <img src="${fullUrl}/ProductPictures/${product.picture}" style=" width:100px;height:100px">
                    </a>
                    <div class="w-100 ps-3">
                        <h5 class="fs-sm fw-medium lh-base mb-2">
                            <a class="hover-effect-underline" href="${fullUrl}/Product/${product.slug}">${product.name}</a>
                        </h5>
                        <div class="h6 pb-1 mb-2">${product.price}</div>
                        <div class="d-flex align-items-center justify-content-between">
                            <div class="count-input rounded-pill">
                                <button type="button" class="btn btn-icon btn-sm" data-decrement="" aria-label="Decrement quantity">
                                    <i class="ci-minus"></i>
                                </button>
                                <input type="number" class="form-control form-control-sm" value="3" readonly="">
                                <button type="button" class="btn btn-icon btn-sm" data-increment="" aria-label="Increment quantity">
                                    <i class="ci-plus"></i>
                                </button>
                            </div>
                            <button type="button" class="btn-close fs-sm" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-sm" data-bs-title="Remove" aria-label="Remove from cart"></button>
                        </div>
                    </div>
             </div >
            `
        cart_items_wrapper.innerHTML += res;
    });
}