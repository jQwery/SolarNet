import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentsIdentComponent } from './comments-ident.component';

describe('CommentsIdentComponent', () => {
  let component: CommentsIdentComponent;
  let fixture: ComponentFixture<CommentsIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommentsIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentsIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
