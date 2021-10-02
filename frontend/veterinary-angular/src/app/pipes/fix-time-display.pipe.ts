import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'displayZeroTime',
})
export class FixTimeDisplayPipe implements PipeTransform {
  transform(time: number): string {
    return time < 10 ? `0${time}` : `${time}`;
  }
}
