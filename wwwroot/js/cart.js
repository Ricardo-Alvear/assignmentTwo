async function addToCart(eventId, ticketTypeId, quantity){
    const res = await fetch('/Cart/Add', {
        method: 'POST',
        headers: {'Content-Type' : 'application/json'},
        body: JSON.stringify({eventId, ticketTypeId, quantity})
    });
    const data = await res.json();
    document.querySelector('#cartCount').innerText = data.count;
    document.querySelector('#cartTotal').innerText = data.total;
    if (data.lowStock) showLowStockWarning(data.remaining);
}