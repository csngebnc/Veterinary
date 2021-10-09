import { PagedListOfVeterinaryUserDto } from './../../../services/generated-api-code';
import { MatTableDataSource } from '@angular/material/table';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { UserService, VeterinaryUserDto } from 'src/app/services/generated-api-code';

@Component({
  selector: 'app-search-users',
  templateUrl: './search-users.component.html',
  styleUrls: ['./search-users.component.scss'],
})
export class SearchUsersComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  pageEvent: PageEvent;
  length: number = 0;
  displayedColumns: string[] = ['userName', 'email', 'button'];

  private searchParamChanged: Subscription;
  searchParamControl = new FormControl('');
  users: VeterinaryUserDto[] = [];
  dataSource: MatTableDataSource<VeterinaryUserDto> = new MatTableDataSource<VeterinaryUserDto>([]);

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers({ pageIndex: 0, pageSize: 10, length: 0 });
    this.searchParamChanged = this.searchParamControl.valueChanges
      .pipe(debounceTime(1000), distinctUntilChanged())
      .subscribe(() => {
        this.loadUsers({ pageIndex: 0, pageSize: 10, length: 0 });
      });
  }

  ngOnDestroy(): void {
    this.searchParamChanged?.unsubscribe();
  }

  loadUsers(event: PageEvent): void {
    this.userService
      .searchUsersPaged(this.searchParamControl.value, event.pageSize, event.pageIndex)
      .subscribe((usersPagedList: PagedListOfVeterinaryUserDto) => {
        this.users = usersPagedList.items;
        this.length = usersPagedList.totalCount;
        this.dataSource.data = this.users;
      });
  }

  pageChanged(event: PageEvent): PageEvent {
    this.userService
      .searchUsersPaged(this.searchParamControl.value, event.pageSize, event.pageIndex)
      .subscribe((usersPagedList: PagedListOfVeterinaryUserDto) => {
        this.users = usersPagedList.items;
        this.length = usersPagedList.totalCount;
        this.dataSource.data = this.users;
      });

    return event;
  }
}
