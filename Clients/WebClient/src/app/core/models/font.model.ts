const url = `https://fonts.googleapis.com/css?family=`;

const css = `* {
  font-family: #!fontname!#;
}`;

const withoutCss = '';

export interface Font {
  link: string;
  css: string;
}

export interface FontByKey {
  key: string;
  font: Font;
}

const fonts: FontByKey[] = [
  {
    key: 'Default',
    font: {
      link: 'Roboto:300,400,500&display=swap',
      css: ``
    }
  },
  {
    key: 'Roboto',
    font: {
      link: 'Roboto&display=swap',
      css: `'Roboto', sans-serif`
    }
  },
  {
    key: 'Mansalva',
    font: {
      link: 'Mansalva&display=swap',
      css: `'Mansalva', cursive`
    }
  },
  {
    key: 'Open Sans',
    font: {
      link: 'Open+Sans&display=swap',
      css: `'Open Sans', sans-serif`
    }
  },
  {
    key: 'Lato',
    font: {
      link: 'Lato&display=swap',
      css: `'Lato', sans-serif`
    }
  },
  {
    key: 'Hepta Slab',
    font: {
      link: 'Hepta+Slab&display=swap',
      css: `'Hepta Slab', serif`
    }
  },
  {
    key: 'Montserrat',
    font: {
      link: 'Montserrat&display=swap',
      css: `'Montserrat', sans-serif`
    }
  },
  {
    key: 'Roboto Condensed',
    font: {
      link: 'Roboto+Condensed&display=swap',
      css: `'Roboto Condensed', sans-serif`
    }
  },
  {
    key: 'Ubuntu',
    font: {
      link: 'Ubuntu&display=swap',
      css: `'Ubuntu', sans-serif`
    }
  }
];

export const fontVariables = {
  url,
  css,
  withoutCss,
  fonts
};
