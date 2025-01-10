document.getElementById('update-movie-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    
    data.IsSubscriptionBased = data.IsSubscriptionBased === 'on';
    data.GenreId = parseInt(data.GenreId);
    data.Rating = parseFloat(data.Rating.replace(",", "."))
    data.ReleaseYear = parseInt(data.ReleaseYear);
    data.Id = parseInt(data.Id);
    const response = await fetch(form.action, {
        method: form.method,
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });
    
    try {
        const result = await response.json();
        if (result.Message != null){
            alert("Error " + result.Message)
            return;
        }
    }
    catch (error){
        console.log(error);
    }
    
    window.location.href = "/admin"
});