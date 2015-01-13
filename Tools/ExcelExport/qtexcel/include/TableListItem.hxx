#ifndef TABLELISTITEM_H
#define TABLELISTITEM_H

#include <QWidget>
#include "CTask.hxx"

namespace Ui
{
	class TableListItem;
}

class TableListItem : public QWidget
{
	Q_OBJECT
private:
	Ui::TableListItem *m_ui;
	Table* m_table;

private slots:
	void onChkClk();

public:
	explicit TableListItem(QWidget *parent);
	void setTable(Table* table);

signals:
	public slots :
};

#endif // TABLELISTITEM_H