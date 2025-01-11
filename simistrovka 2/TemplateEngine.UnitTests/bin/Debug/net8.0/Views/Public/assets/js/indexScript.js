document.getElementById('add-movie-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    data.IsSubscriptionBased = data.IsSubscriptionBased === 'on'; // Преобразуем чекбокс в true/false
    data.GenreId = parseInt(data.GenreId);
    data.Rating = parseFloat(data.Rating);
    data.ReleaseYear = parseInt(data.ReleaseYear);
    try {
        const response = await fetch(form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result === false) {
            alert('Такой фильм уже есть');
            throw new Error('Такой фильм уже есть');
        }
        
        if (result.Message != null){
            alert("Error " + result.Message)
            return;
        }

        const newRow = document.createElement('tr');
        newRow.innerHTML = `
            <tr>
                <td>${result.Id}</td>
                <td>${result.Title}</td>
                <td>${result.Description}</td>
                <td>${result.GenreId}</td>
                <td>${result.Rating}</td>
                <td>${result.ReleaseYear}</td>
                <td>${result.Duration}</td>
                <td>${result.IsSubscriptionBased}</td>
                <td>${result.VerticalImageUrl}</td>
                <td>${result.HorizontalImageUrl}</td>
                <td>
                    <form action="admin/movies/update" method="get">
                        <input type="hidden" name="id" value="${result.Id}" />
                        <button type="submit" style="all: unset;">
                            Update
                        </button>
                    </form>
                </td>
            </tr>
        `;
        document.getElementById('movies-table').appendChild(newRow);
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении фильма:', error);
    }
});

document.getElementById('add-actor-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    try {
        const response = await fetch(form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такой актер уже существует');
            throw new Error('Такой актер уже существует');
        }

        if (result.Message != null){
            alert("Error " + result.Message)
            return;
        }

        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.Name}</td>
                <td>${result.PhotoUrl}</td>
                <td>${result.Biography}</td>
                <td>
                    <form action="admin/actors/update" method="get">
                        <input type="hidden" name="id" value="${result.Id}" />
                        <button type="submit" style="all: unset;">
                            Update
                        </button>
                    </form>
                </td>
            </tr>
        `;

        const table = document.getElementById('actors-table');
        table.appendChild(newRow);
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении актера:', error);
    }
});

document.getElementById('add-movie-actor-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    // Преобразуем числовые поля
    data.MovieId = parseInt(data.MovieId);
    data.ActorId = parseInt(data.ActorId);

    try {
        const response = await fetch(form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такая связь уже существует');
            throw new Error('Такая связь уже существует');
        }

        if (result.Message != null){
            alert("Error " + result.Message)
            return;
        }

        if(result == 1){
            alert("некорректная связь");
            throw new Error('incorrect constraint');
        }

        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.MovieId}</td>
                <td>${result.ActorId}</td>
                <td>
                    <form action="admin/movieactors/update" method="get">
                        <input type="hidden" name="id" value="${result.Id}" />
                        <button type="submit" style="all: unset;">
                            Update
                        </button>
                    </form>
                </td>
            </tr>
        `;

        const table = document.getElementById('movie-actors-table');
        table.appendChild(newRow);
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении связи фильм-актер:', error);
    }
});

document.getElementById('add-genre-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    try {
        const response = await fetch(form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такой жанр уже существует');
            throw new Error('Такой жанр уже существует');
        }

        if (result.Message != null){
            alert("Error " + result.Message)
            return;
        }

        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.Name}</td>
                <td>
                    <form action="admin/genres/update" method="get">
                        <input type="hidden" name="id" value="${result.Id}" />
                        <button type="submit" style="all: unset;">
                            Update
                        </button>
                    </form>
                </td>
            </tr>
        `;

        const table = document.getElementById('genres-table');
        table.appendChild(newRow);
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении жанра:', error);
    }
});

document.getElementById('add-user-form').addEventListener('submit', async function (event) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    // Проверка логина (email)
    if (!validateEmail(data.UserName)) {
        alert("Некорректный username. Пожалуйста, введите правильный адрес электронной почты.");
        return;
    }

    // Проверка пароля
    if (!validatePassword(data.Password)) {
        alert("Пароль должен содержать минимум 8 символов, одну цифру, одну заглавную букву и не содержать специальных символов.");
        return;
    }
    
    // Преобразуем числовые поля
    data.IsAdmin = data.IsAdmin === 'on';

    try {
        const response = await fetch(form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такой пользователь уже существует');
            throw new Error('Такой пользователь уже существует');
        }

        if (result.Message != null){
            alert("Error " + result.Message)
            return;
        }

        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.UserName}</td>
                <td>${result.Password}</td>
                <td>${result.IsAdmin}</td>
                <td>
                    <form action="admin/users/update" method="get">
                        <input type="hidden" name="id" value="${result.Id}" />
                        <button type="submit" style="all: unset;">
                            Update
                        </button>
                    </form>
                </td>
            </tr>
        `;

        const table = document.getElementById('users-table');
        table.appendChild(newRow);
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении пользователя:', error);
    }
});

function validatePassword(password) {
    const passwordRegex = /^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$/;
    return passwordRegex.test(password);
}

function validateEmail(email) {
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return emailRegex.test(email);
}