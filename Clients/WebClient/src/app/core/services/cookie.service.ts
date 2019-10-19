import { Injectable } from '@angular/core';

export interface Cookie {
  key: string;
  value: string;
}

let cookies: Cookie[] = [];

@Injectable({ providedIn: 'root' })
export class CookieService {
  getAntiforgery() {
    const antiforgeryKey = this.keys().filter(ck => ck.toLowerCase().includes('antiforgery'))[0];
    const value = this.get(antiforgeryKey);
    return value;
  }

  get(key: string) {
    const name = key + '=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < ca.length; i++) {
      let c = ca[i];
      while (c.charAt(0) === ' ') {
        c = c.substring(1);
      }

      if (c.indexOf(name) === 0) {
        return c.substring(name.length, c.length);
      }
    }

    return '';
  }

  empty(): boolean {
    return document.cookie === '';
  }

  setAll(): void {
    cookies = document.cookie.split('; ').map((c: string) => {
      const cookieKVP = c.split('=');
      return { key: cookieKVP[0], value: cookieKVP[1] };
    });
  }

  keys(): string[] {
    return cookies.map(c => c.key);
  }

  values(): string[] {
    return cookies.map(c => c.value);
  }

  value(key: string): Cookie {
    const filtredCookies = cookies.filter(c => c.key === key);
    return filtredCookies.length > 0 ? filtredCookies[0] : null;
  }
}
