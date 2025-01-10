document.getElementById('update-genre-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    // Преобразуем Id в число
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
    
    if (response.ok) {
        // После успешного запроса перенаправляем на страницу со списком жанров
        window.location.href = "/admin"; // Перенаправление на список жанров
    } else {
        console.error('Ошибка при обновлении жанра:', response.statusText);
        // Обработка ошибки или уведомление пользователя
    }
});
