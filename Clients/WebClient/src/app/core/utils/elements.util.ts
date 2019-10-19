function getStylePropertyValue(element: HTMLElement, property: string): string {
  return window.getComputedStyle(element, null).getPropertyValue(property);
}

function getStyleValueAndConvertedToNumber(element: HTMLElement, property: string) {
  const originalValue = getStylePropertyValue(element, property);
  const numberValue = Number(originalValue.match(/\d+/g)[0]);
  return { originalValue, numberValue };
}

function increaseStylePropertyValue(element: HTMLElement, property: string, increaseWith: number) {
  const vcnv = getStyleValueAndConvertedToNumber(element, property);
  vcnv.numberValue += increaseWith;
  vcnv.originalValue = vcnv.originalValue.replace(/\d+/g, vcnv.numberValue.toString());
  element.style[property] = vcnv.originalValue;
}

function decreaseStylePropertyValue(element: HTMLElement, property: string, decreaseWith: number) {
  const vcnv = getStyleValueAndConvertedToNumber(element, property);
  vcnv.numberValue -= decreaseWith;
  vcnv.originalValue = vcnv.originalValue.replace(/\d+/g, vcnv.numberValue.toString());
  element.style[property] = vcnv.originalValue;
}

function getScrollbarWidth() {
  const outer = document.createElement('div');
  outer.style.visibility = 'hidden';
  outer.style.overflow = 'scroll';
  outer.style.msOverflowStyle = 'scrollbar';
  document.body.appendChild(outer);
  const inner = document.createElement('div');
  outer.appendChild(inner);
  const scrollbarWidth = (outer.offsetWidth - inner.offsetWidth);
  outer.parentNode.removeChild(outer);

  return scrollbarWidth;
}

export const functions = {
  getStylePropertyValue: (element: HTMLElement, property: string) => getStylePropertyValue(element, property),
  increaseStylePropertyValue: (element: HTMLElement, property: string, increaseWith: number) =>
                                              increaseStylePropertyValue(element, property, increaseWith),
  decreaseStylePropertyValue: (element: HTMLElement, property: string, decreaseWith: number) =>
                              decreaseStylePropertyValue(element, property, decreaseWith),
  getScrollbarWidth: () => getScrollbarWidth()
};
