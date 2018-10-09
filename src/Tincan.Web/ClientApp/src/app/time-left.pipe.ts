import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'timeLeft'
})
export class TimeLeftPipe implements PipeTransform {

  transform(value: number, args?: any): any {
    if (value < 60) {
      const secondsLeft = Math.round(value);
      return `${secondsLeft} seconds`;
    } else if (value < 3600) {
      const minutesLeft = Math.round(value / 6) / 10;
      return `${minutesLeft} minute${minutesLeft > 1 ? 's' : ''}`;
    } else if (value < 86400) {
      const hoursLeft = Math.round(value / 360) / 10;
      return `${hoursLeft} hour${hoursLeft > 1 ? 's' : ''}`;
    } else {
      const daysLeft = Math.round(value / 8640) / 10;
      return `${daysLeft} day${daysLeft > 1 ? 's' : ''}`;
    }
  }
}
