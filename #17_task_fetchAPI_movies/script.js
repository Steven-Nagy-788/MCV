const index = document.getElementById('index');
const UpComingMoviesDiv = document.getElementById('UpComingMoviesDiv');
const NowInCinemaDiv = document.getElementById('NowInCinemaDiv');

let UpComingMovies = [];
let NowInCinema = [];
let MoviesGenres = [];

async function getData () {
  const options = {
    method: 'GET',
    headers: {
      accept: 'application/json',
      Authorization: 'Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI3NzdhNmFhNGI1N2Q1MmU1OWRjZWE5YTNkOThiNTExOCIsIm5iZiI6MTc1Njc1MjEwNi44ODQsInN1YiI6IjY4YjVlOGVhNjg3OTlhNDY0MmI0ZjkwYyIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.EZnxhUwpWwo3avOC_LQTIm36VFRcIXWxAUcDRa-xp6g'
    }
  };

  await fetch('https://api.themoviedb.org/3/movie/upcoming?language=en-US&page=1', options)
    .then(res => res.json())
    .then(res => {
      UpComingMovies = res.results;
    })
    .catch(err => console.error(err));

  await fetch('https://api.themoviedb.org/3/movie/now_playing?language=en-US&page=1', options)
    .then(res => res.json())
    .then(res => {
      NowInCinema = res.results;
    })
    .catch(err => console.error(err));

  await fetch('https://api.themoviedb.org/3/genre/movie/list?language=en', options)
    .then(res => res.json())
    .then(res => {
      MoviesGenres = res.genres;
    })
    .catch(err => console.error(err));
}
async function intializeData() {
  await getData();
  index.innerHTML += `
    <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
  `;
  UpComingMoviesDiv.innerHTML += `
    <div class="carousel-item active">
  <img src="https://image.tmdb.org/t/p/w500${UpComingMovies[0].poster_path}" class="d-block mx-auto img-fluid rounded" alt="${UpComingMovies[0].title}">
      <div class="carousel-caption d-none d-md-block">
        <h5>${UpComingMovies[0].title}</h5>
        <p>${UpComingMovies[0].overview}</p>
      </div>
    </div>
  `;
  for (let i = 1; i < UpComingMovies.length; i++) {
    index.innerHTML += `
      <button type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide-to="${i}" aria-label="Slide ${i + 1}"></button>
    `;
    UpComingMoviesDiv.innerHTML += `
      <div class="carousel-item">
  <img src="https://image.tmdb.org/t/p/w500${UpComingMovies[i].poster_path}" class="d-block mx-auto img-fluid rounded" alt="${UpComingMovies[i].title}">
        <div class="carousel-caption bg-dark d-none d-md-block" style="--bs-bg-opacity: .3;">
          <h5>${UpComingMovies[i].title}</h5>
          <p>${UpComingMovies[i].overview}</p>
        </div>
      </div>
    `;
  }
  for (let i = 0; i < NowInCinema.length; i++) {
    NowInCinemaDiv.innerHTML += `
      <div class="col">
        <div class="card bg-dark text-white h-100">
          <img src="https://image.tmdb.org/t/p/w500${NowInCinema[i].poster_path}" class="card-img" alt="${NowInCinema[i].title}">
          <div class="card-body">
            <h5 class="card-title text-center">${NowInCinema[i].title}</h5>
            <p class="card-text">${NowInCinema[i].overview}</p>
          </div>
          <div class="card-footer text-body-secondary">

            <small class="text-white ">
              ${
                MoviesGenres.filter(genre => NowInCinema[i].genre_ids.find(id => id === genre.id)).map(genre => {
                  return `<span class=" bg-primary m-1 p-1 rounded">${genre.name}</span>`;
                }).join('')
              }
            </small>
          </div>
        </div>
      </div>
    `;
  }
}

intializeData();