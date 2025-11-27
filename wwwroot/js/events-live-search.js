document.querySelector("#eventSearchInput").addEventListener("input", async function (e){ 
    const q = e.target.value;
    const res = await fetch('/Events/Search?q=${encodeURIComponent(q)}');
    const html = await res.text();
    document.querySelector("#eventsContainer").innerHTML = html;
})