const html = document.querySelector('html');
const theme = localStorage.getItem('theme');
if (theme) html.dataset.bsTheme = theme;
else {
    const darkModeMediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    if (darkModeMediaQuery.matches) html.dataset.bsTheme = 'dark';
    else html.dataset.bsTheme = 'light';
}

function toggleDarkMode() {
    const html = document.querySelector('html');
    if (html.dataset.bsTheme === 'dark') {
        html.dataset.bsTheme = 'light';
    }
    else {
        html.dataset.bsTheme = 'dark';
    }
    localStorage.setItem('theme', html.dataset.bsTheme);
}