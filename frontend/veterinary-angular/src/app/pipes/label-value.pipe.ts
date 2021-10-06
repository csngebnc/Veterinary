import { LabelValuePairOfAppointmentStatusEnum } from './../services/generated-api-code';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'labelValue',
})
export class LabelValuePipe implements PipeTransform {
  transform(incomingValue: number, pairs: LabelValuePairOfAppointmentStatusEnum[]): string {
    return pairs.find(({ value }) => value == incomingValue).label;
  }
}
