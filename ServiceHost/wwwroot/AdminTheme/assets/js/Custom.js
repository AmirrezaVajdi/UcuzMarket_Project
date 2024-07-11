const cookieName = "cart-items";

function addToCart(Id, Name, price, Picture) {
    let products = $.cookie(cookieName);
    if (products === undefined) {
        products = [];
    } else {
        products = JSON.parse(products);
    }

    const count = $("#productCount").val();
    const currentProduct = products.find(x => x.Id === Id);
    if (currentProduct !== undefined) {
        products.find(x => x.Id === Id).Count = parseInt(currentProduct.Count) + parseInt(count);
    } else {
        const product = {
            Id,
            Name,
            UnitPrice: price,
            Picture,
            Count: count
        }

        products.push(product);
    }

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
}

function updateCart() {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    $("#cart_items_count").text(products.length);
    const cartItemsWrapper = $("#cart_items_wrapper");
    cartItemsWrapper.html('');
    products.forEach(x => {
        const product = `<div class="single-cart-item">
                            <a href="javascript:void(0)" class="remove-icon" onclick="removeFromCart('${x.id}')">
                                <i class="ion-android-close"></i>
                            </a>
                            <div class="image">
                                <a href="single-product.html">
                                    <img src="/ProductPictures/${x.Picture}"
                                         class="img-fluid" alt="">
                                </a>
                            </div>
                            <div class="content">
                                <p class="product-title">
                                    <a href="single-product.html">محصول: ${x.Name}</a>
                                </p>
                                <p class="count">تعداد: ${x.Count}</p>
                                <p class="count">قیمت واحد: ${x.UnitPrice}</p>
                            </div>
                        </div>`;

        cartItemsWrapper.append(product);
    });
}

function removeFromCart(id) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    const itemToRemove = products.findIndex(x => x.id === id);
    products.splice(itemToRemove, 1);
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
}


function changeCartItemCount(id, totalId, count) {
    var products = $.cookie(cookieName);
    products = JSON.parse(products);
    var productIndex = products.findIndex(x => x.Id == id);
    products[productIndex].Count = count;
    var product = products[productIndex];
    const newPrice = parseInt(product.UnitPrice) * parseInt(count);
    $(`#${totalId}`).text(newPrice);
    products[productIndex].totalPrice = newPrice;
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();

    const settings = {
        "url": "https://localhost:7211/api/inventory",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({ "productId": id, "count": count })
    };

    $.ajax(settings).done(function (data) {
        if (data.inStock == false) {
            const warningsDiv = $('#productStockWarnings');
            if ($(`#${id}`).length == 0) {
                warningsDiv.append(`
                    <div class="alert alert-warning" id="${id}">
                        <i class="fa fa-warning"></i> کالای
                        <strong>${data.productName}</strong>
                        در انبار کمتر از تعداد درخواستی موجود است.
                    </div>
                `);
            }
        } else {
            if ($(`#${id}`).length > 0) {
                $(`#${id}`).remove();
            }
        }
    });
}

function GetCartCount() {
    var products = $.cookie(cookieName);
    products = JSON.parse(products);
    $("#cart_items_count-mobile").text(products.length);
}
