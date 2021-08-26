import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'translateRole',
})
export class TranslateRolePipe implements PipeTransform {
  roleNames = {
    User: 'Gazdi',
    NormalDoctor: 'állatorvos',
    ManagerDoctor: 'vezető-állatorvos',
  };

  transform(roleName: string): string {
    return this.roleNames[roleName];
  }
}
